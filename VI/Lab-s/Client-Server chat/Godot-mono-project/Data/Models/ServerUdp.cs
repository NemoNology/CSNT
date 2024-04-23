using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ServerUdp : Server
    {
        private readonly List<IPEndPoint> _clientsIpEndPoints = new(2);

        public override event Action<byte[]> MessageReceived;

        public ServerUdp()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public override void Start(IPAddress ipAddress, int port)
        {
            if (_isRunning || _isDisposed)
                return;

            _socket.Bind(new IPEndPoint(ipAddress, port));
            _isRunning = true;
            _messagesBytes.Add(Encoding.UTF8.GetBytes($"Сервер ({_socket.LocalEndPoint}) ({DateTime.Now}) запущен"));
            // Add init massage to messages
            SendLastMessageToClients();
            // Thread for receiving messages
            Task.Run(() =>
            {
                byte[] buffer = new byte[4096];
                while (_isRunning)
                {
                    EndPoint networkEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var reciedBytesLength = _socket.ReceiveFrom(buffer, ref networkEndPoint);
                    IPEndPoint clientIpEndPoint = SocketAddressToEndPoint(networkEndPoint.Serialize());
                    bool isNewClient = !_clientsIpEndPoints.Contains(clientIpEndPoint);

                    // Check if it's new client
                    if (isNewClient)
                    {
                        // Send new client all messages
                        foreach (byte[] msg in _messagesBytes)
                            _socket.SendTo(msg, networkEndPoint);
                        // Add new client
                        _clientsIpEndPoints.Add(clientIpEndPoint);
                        lock (_messagesBytes)
                        {
                            _messagesBytes.Add(
                                Encoding.UTF8.GetBytes(
                                    $"{clientIpEndPoint} ({DateTime.Now}) подключился"));
                        }
                        // Notify clients about new client connection
                        SendLastMessageToClients();
                    }

                    // Empty message mean dissconectin or connection
                    if (!isNewClient && reciedBytesLength == 0)
                    {
                        lock (_clientsIpEndPoints)
                        {
                            _clientsIpEndPoints.Remove(clientIpEndPoint);
                        }
                        lock (_messagesBytes)
                        {
                            _messagesBytes.Add(
                                Encoding.UTF8.GetBytes(
                                    $"{clientIpEndPoint} ({DateTime.Now}) отключился"));
                        }

                        SendLastMessageToClients();
                    }
                    // Add new message if it's not empty
                    else if (reciedBytesLength > 0)
                    {
                        lock (_messagesBytes)
                        {
                            _messagesBytes.Add(Encoding.UTF8.GetBytes(
                                $"{clientIpEndPoint} ({DateTime.Now}): ")
                                .Concat(buffer[..reciedBytesLength])
                                .ToArray());
                        }

                        SendLastMessageToClients();
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        public override void Stop()
        {
            if (!_isRunning || _isDisposed)
                return;

            _messagesBytes.Add(Array.Empty<byte>());
            SendLastMessageToClients();
            _isRunning = false;
            lock (_messagesBytes)
            {
                _messagesBytes.Clear();
                _messagesBytes.Capacity = 16;
            }
            _cancellationTokenSource.Cancel();
        }

        private void SendLastMessageToClients()
        {
            if (!_isRunning || _isDisposed)
                return;

            MessageReceived?.Invoke(_messagesBytes[^1]);
            foreach (IPEndPoint endPoint in _clientsIpEndPoints)
                _socket.SendTo(_messagesBytes[^1], endPoint);
        }

        private static IPEndPoint SocketAddressToEndPoint(SocketAddress socketAddress)
        {
            // First two bytes in socket address are address family
            // Two bytes after are port
            // Four bytes after port are ip address
            return new IPEndPoint(
                0 << 32 | socketAddress[7] << 24 | socketAddress[6] << 16 | socketAddress[5] << 8 | socketAddress[4],
                0 << 16 | socketAddress[2] << 8 | socketAddress[3]);
        }
    }
}