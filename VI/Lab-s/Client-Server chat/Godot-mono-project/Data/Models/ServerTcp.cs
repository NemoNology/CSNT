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

        public ServerTcp()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public override void Start(IPAddress ipAddress, int port)
        {
            if (_isRunning)
                return;

            var serverEndPoint = new IPEndPoint(ipAddress, port);
            _socket.Bind(serverEndPoint);
            IsRunning = true;
            // Add init massage to messages
            _messagesBytes.Add(
                Encoding.UTF8.GetBytes(GetInitializeMessage(serverEndPoint)));
            SendLastMessageToClients();
            // Thread for accepting connections and listening clients
            Task.Run(() =>
            {
                while (_isRunning)
                {
                    // Start listening
                    _socket.Listen();
                    var clientSocket = _socket.Accept();
                    lock (_messagesBytes)
                    {
                        // Send all messages for new client
                        foreach (byte[] msg in _messagesBytes)
                        {
                            clientSocket.Send(msg);
                        }
                        _messagesBytes.Add(
                            Encoding.UTF8.GetBytes(
                                GetClientConnectedMessage(clientSocket.RemoteEndPoint)
                            ));
                    }

                    lock (_clientsSockets)
                        _clientsSockets.Add(clientSocket);

                    // Notify about new client
                    SendLastMessageToClients();

                    // Start receive messages from connected client in another thread
                    Task.Run(() =>
                    {
                        byte[] buffer = new byte[NetHelper.BUFFERSIZE];
                        int receivedBytesLength;
                        while (clientSocket.Connected)
                        {
                            receivedBytesLength = clientSocket.Receive(buffer);
                            var receivedBytes = buffer[..receivedBytesLength];

                            if (Enumerable.SequenceEqual(receivedBytes, NetHelper.SpecialMessageBytes))
                            {
                                lock (_clientsSockets)
                                    _clientsSockets.Remove(clientSocket);
                                lock (_messagesBytes)
                                {
                                    _messagesBytes.Add(
                                        Encoding.UTF8.GetBytes(
                                            GetClientDisconnectedMessage(clientSocket.RemoteEndPoint)));
                                }
                            }
                            else
                            {
                                _messagesBytes.Add(Encoding.UTF8.GetBytes(
                                    GetClientFormattedMessage(
                                        clientSocket.RemoteEndPoint, buffer[..receivedBytesLength])
                                    ));
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
                _messagesBytes.Add(NetHelper.SpecialMessageBytes);
                SendLastMessageToClients();
                _messagesBytes.Clear();
                _messagesBytes.Capacity = 16;
            }
            IsRunning = false;
            lock (_clientsSockets)
            {
                _clientsSockets.Clear();
                _clientsSockets.Capacity = 2;
            }
            _cancellationTokenSource.Cancel();
        }

        private void SendLastMessageToClients()
        {
            if (!_isRunning)
                return;

            MessageReceived?.Invoke(_messagesBytes[^1]);

            lock (_clientsSockets)
            {
                foreach (Socket socket in _clientsSockets)
                {
                    lock (socket)
                    {
                        if (socket.SafeHandle.IsClosed || !socket.Connected)
                        {
                            _clientsSockets.Remove(socket);
                            continue;
                        }

                        socket.Send(_messagesBytes[^1]);
                        socket.Close();
                    }
                }
            }
        }
    }
}