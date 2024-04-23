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
        private readonly List<Socket> _clientsSockets = new(2);

        public ServerTcp()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public override event Action<byte[]> MessageReceived;

        public override void Start(IPAddress ipAddress, int port)
        {
            if (_isRunning)
                return;

            _socket.Bind(new IPEndPoint(ipAddress, port));
            _isRunning = true;
            _messagesBytes.Add(Encoding.UTF8.GetBytes($"Server ({_socket.LocalEndPoint}) ({DateTime.Now}) started"));
            MessageReceived?.Invoke(_messagesBytes[^1]);
            // Thread for accepting connections
            Task.Run(async () =>
            {
                while (_isRunning)
                {
                    _socket.Listen();
                    var clientSocket = await _socket.AcceptAsync();
                    lock (_messagesBytes)
                    {
                        foreach (byte[] msg in _messagesBytes)
                            clientSocket.Send(msg);
                        _messagesBytes.Add(Encoding.UTF8.GetBytes($"{clientSocket.LocalEndPoint} ({DateTime.Now}) connected"));
                    }
                    lock (_clientsSockets)
                    {
                        _clientsSockets.Add(clientSocket);
                        SendLastMessageToClients();
                    }
                }
            }, _cancellationTokenSource.Token);
            // Thread for check clients disconnections
            Task.Run(() =>
            {
                while (_isRunning)
                {
                    foreach (Socket socket in _clientsSockets)
                    {
                        if (!socket.SafeHandle.IsClosed)
                        {
                            lock (_messagesBytes)
                                _messagesBytes.Add(
                                    Encoding.UTF8.GetBytes(
                                        $"{socket.LocalEndPoint} ({DateTime.Now}) disconnected"));
                            lock (_clientsSockets)
                                _clientsSockets.Remove(socket);
                            SendLastMessageToClients();
                        }
                    }
                }
            }, _cancellationTokenSource.Token);
            // Thread for receiving messages
            Task.Run(() =>
            {
                byte[] buffer = new byte[4096];
                while (_isRunning)
                {
                    foreach (Socket socket in _clientsSockets)
                    {
                        if (!socket.SafeHandle.IsClosed && socket.Available > 0)
                        {
                            var reciedBytesLength = socket.Receive(buffer);
                            lock (_messagesBytes)
                                _messagesBytes.Add(
                                    Encoding.UTF8.GetBytes(
                                        $"{socket.LocalEndPoint} ({DateTime.Now}): ")
                                        .Concat(buffer[..reciedBytesLength])
                                        .ToArray());
                            SendLastMessageToClients();
                        }
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        public override void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;
            lock (_messagesBytes)
            {
                _messagesBytes.Clear();
                _messagesBytes.Capacity = 16;
            }
            lock (_clientsSockets)
            {
                foreach (Socket socket in _clientsSockets)
                {
                    if (!socket.SafeHandle.IsClosed)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                }
                _clientsSockets.Clear();
                _clientsSockets.Capacity = 2;
            }
            _cancellationTokenSource.Cancel();
        }

        private void SendLastMessageToClients()
        {
            MessageReceived?.Invoke(_messagesBytes[^1]);
            foreach (Socket socket in _clientsSockets)
            {
                if (socket.SafeHandle.IsClosed)
                    continue;

                lock (socket)
                {
                    socket.Send(_messagesBytes[^1]);
                }
            }
        }
    }
}