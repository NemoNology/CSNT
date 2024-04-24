using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ServerTcp : Server
    {
        private readonly List<Socket> _clientsSockets = new(4);

        public override event Action<byte[]> MessageReceived;

        public static readonly byte[] CloseMessageBytes = new byte[] { 0 };

        public ServerTcp()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public override void Start(IPAddress ipAddress, int port)
        {
            if (_isRunning || _isDisposed)
                return;

            _socket.Bind(new IPEndPoint(ipAddress, port));
            _isRunning = true;
            // Add init massage to messages
            _messagesBytes.Add(Encoding.UTF8.GetBytes($"Сервер ({_socket.LocalEndPoint}) ({DateTime.Now}) запущен\n"));
            SendLastMessageToClients();
            // Thread for accepting connections and listening clients
            Task.Run(() =>
            {
                while (_isRunning)
                {
                    _socket.Listen();
                    var clientSocket = _socket.Accept();
                    lock (_messagesBytes)
                    {
                        foreach (byte[] msg in _messagesBytes)
                        {
                            clientSocket.Send(msg);
                        }
                        _messagesBytes.Add(Encoding.UTF8.GetBytes($"{clientSocket.RemoteEndPoint} ({DateTime.Now}) подключился\n"));
                    }
                    lock (_clientsSockets)
                    {
                        _clientsSockets.Add(clientSocket);
                        SendLastMessageToClients();
                    }

                    // Start recieve messages from connected client in another thread
                    Task.Run(async () =>
                    {
                        byte[] buffer = new byte[2048];
                        int recievedBytesLength;
                        while (clientSocket.Connected)
                        {
                            recievedBytesLength = await clientSocket.ReceiveAsync(buffer, SocketFlags.None, _cancellationTokenSource.Token);

                            // Empty message mean dissconectin or connection
                            if (recievedBytesLength == 0)
                            {
                                lock (_clientsSockets)
                                    _clientsSockets.Remove(clientSocket);
                                lock (_messagesBytes)
                                {
                                    _messagesBytes.Add(
                                        Encoding.UTF8.GetBytes(
                                            $"{clientSocket.RemoteEndPoint} ({DateTime.Now}) отключился\n"));
                                }
                                SendLastMessageToClients();
                                return;
                            }
                            // If message is not empty then just add it to messages
                            else if (recievedBytesLength > 0)
                            {
                                lock (_messagesBytes)
                                {
                                    _messagesBytes.Add(
                                        Encoding.UTF8.GetBytes(
                                            $"{clientSocket.RemoteEndPoint} ({DateTime.Now}): ")
                                            .Concat(buffer[..recievedBytesLength]
                                            .Concat(Encoding.UTF8.GetBytes("\n")))
                                            .ToArray());
                                }
                            }

                            SendLastMessageToClients();
                        }
                    }, _cancellationTokenSource.Token);
                }
            }, _cancellationTokenSource.Token);
        }

        public override void Stop()
        {
            if (!_isRunning)
                return;

            lock (_messagesBytes)
            {
                _messagesBytes.Add(CloseMessageBytes);
                SendLastMessageToClients();
                _messagesBytes.Clear();
                _messagesBytes.Capacity = 16;
            }
            _isRunning = false;
            lock (_clientsSockets)
            {
                _clientsSockets.Clear();
                _clientsSockets.Capacity = 2;
            }
            _cancellationTokenSource.Cancel();
        }

        private void SendLastMessageToClients()
        {
            if (!_isRunning || _isDisposed)
                return;

            MessageReceived?.Invoke(_messagesBytes[^1]);

            lock (_clientsSockets)
            {
                foreach (Socket socket in _clientsSockets)
                {
                    lock (socket)
                    {
                        if (socket.SafeHandle.IsClosed || !socket.Connected)
                            continue;

                        socket.Send(_messagesBytes[^1]);
                    }
                }
            }
        }
    }
}