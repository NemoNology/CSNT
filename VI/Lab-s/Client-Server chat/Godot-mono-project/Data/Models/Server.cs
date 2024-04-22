using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class Server
    {
        private readonly Socket _socket;
        private bool _isRunning = false;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly List<Socket> _clientsSockets = new(2);
        private readonly List<byte[]> _messagesBytes = new(16);

        public bool IsRunning => _isRunning;

        public Server(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            _socket = new(addressFamily, socketType, protocolType);
        }

        public void Start(IPAddress ipAddress, int port)
        {
            if (_isRunning)
                return;

            _socket.Bind(new IPEndPoint(ipAddress, port));
            _isRunning = true;
            _messagesBytes.Add(Encoding.UTF8.GetBytes($"Server ({_socket.LocalEndPoint}) ({DateTime.Now}) started"));
            // Thread for accepting connections
            Task.Run(() =>
            {
                while (_isRunning)
                {
                    var clientSocket = _socket.Accept();
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

        public void Stop()
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