using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ServerUdp : Server
    {
        private readonly List<IPEndPoint> _clientsIpEndPoints = new(2);

        public override event Action<byte[]> MessageReceived;

        public override void Start(IPAddress ipAddress, int port)
        {
            if (_isRunning)
                return;

            _socket.Bind(new IPEndPoint(ipAddress, port));
            _isRunning = true;
            _messagesBytes.Add(Encoding.UTF8.GetBytes($"Server ({_socket.LocalEndPoint}) ({DateTime.Now}) started"));
            MessageReceived?.Invoke(_messagesBytes[^1]);
            // Thread for receiving messages
            Task.Run(() =>
            {
                byte[] buffer = new byte[4096];
                while (_isRunning)
                {
                    EndPoint networkEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var reciedBytesLength = _socket.ReceiveFrom(buffer, ref networkEndPoint);
                    IPEndPoint clientIpEndPoint = SocketAddressToEndPoint(networkEndPoint.Serialize());

                    // Check if it's new client
                    if (!_clientsIpEndPoints.Contains(clientIpEndPoint))
                    {
                        // Send new client all messages
                        foreach (byte[] msg in _messagesBytes)
                            _socket.SendTo(msg, networkEndPoint);
                        // Add new client
                        _clientsIpEndPoints.Add(clientIpEndPoint);
                    }
                    // Add new message
                    lock (_messagesBytes)
                    {
                        _messagesBytes.Add(Encoding.UTF8.GetBytes(
                            $"{networkEndPoint.Serialize()} ({DateTime.Now}): ")
                            .Concat(buffer[..reciedBytesLength])
                            .ToArray());
                    }
                    // Send last message to every client
                    SendLastMessageToClients();
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
            _cancellationTokenSource.Cancel();
        }

        private void SendLastMessageToClients()
        {
            foreach (IPEndPoint endPoint in _clientsIpEndPoints)
                _socket.SendTo(_messagesBytes[^1], endPoint);
        }

        private static IPEndPoint SocketAddressToEndPoint(SocketAddress socketAddress)
        {
            // First two bytes in socket address are address family
            // Two bytes after are port
            // Four bytes after port are ip address
            return new IPEndPoint(
                0 << 32 | socketAddress[4] << 24 | socketAddress[5] << 16 | socketAddress[6] << 8 | socketAddress[7],
                0 << 16 | socketAddress[2] << 8 | socketAddress[3]);
        }
    }
}