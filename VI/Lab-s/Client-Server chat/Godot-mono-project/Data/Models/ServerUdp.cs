using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ServerUdp : Server
    {
        private readonly List<EndPoint> _clientsEndPoints = new(2);

        public override event Action<byte[]> MessageReceived;

        public ServerUdp()
        {
            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public override void Start(IPAddress ipAddress, int port)
        {
            if (_isRunning)
                return;

            _socket.Bind(new IPEndPoint(ipAddress, port));
            IsRunning = true;
            // Add init massage to messages
            _messagesBytes.Add(Encoding.UTF8.GetBytes(
                GetInitializeMessage(_socket.LocalEndPoint)));
            SendLastMessageToClients();
            // Thread for receiving messages
            Task.Run(() =>
            {
                var buffer = new byte[4096];
                EndPoint clientEndPoint;
                while (_isRunning)
                {
                    // Start listenning
                    clientEndPoint = new IPEndPoint(IPAddress.Any, port);
                    var recievedBytesLength = _socket.ReceiveFrom(buffer, SocketFlags.None, ref clientEndPoint);
                    bool isNewClient = !_clientsEndPoints.Contains(clientEndPoint);

                    // Check if it's new client
                    if (isNewClient)
                    {
                        // Send all messages for new client
                        foreach (byte[] msg in _messagesBytes)
                            _socket.SendTo(msg, clientEndPoint);
                        // Add new client
                        _clientsEndPoints.Add(clientEndPoint);
                        lock (_messagesBytes)
                        {
                            _messagesBytes.Add(
                                Encoding.UTF8.GetBytes(
                                    GetClientConnectedMessage(clientEndPoint)));
                        }
                        // Notify clients about new client connection
                        SendLastMessageToClients();
                    }

                    lock (_messagesBytes)
                    {
                        if (!isNewClient && Enumerable.SequenceEqual(buffer, NetHelper.SpecialMessageBytes))
                        {
                            _messagesBytes.Add(Encoding.UTF8.GetBytes(
                                GetClientDisconnectedMessage(clientEndPoint)));

                            // Remove disconnected client
                            lock (_clientsEndPoints)
                                _clientsEndPoints.Remove(clientEndPoint);
                        }
                        else
                        {
                            _messagesBytes.Add(
                                GetClientFormattedMessageAsBytes(clientEndPoint, buffer));
                        }
                    }

                    SendLastMessageToClients();
                }
            }, _cancellationTokenSource.Token);
        }

        public override void Stop()
        {
            if (!_isRunning)
                return;

            _messagesBytes.Add(NetHelper.SpecialMessageBytes);
            SendLastMessageToClients();
            IsRunning = false;
            lock (_messagesBytes)
            {
                _messagesBytes.Clear();
                _messagesBytes.Capacity = 16;
            }
            _cancellationTokenSource.Cancel();
        }

        private void SendLastMessageToClients()
        {
            if (!_isRunning)
                return;

            var lastMessage = _messagesBytes[^1];
            MessageReceived?.Invoke(lastMessage);
            foreach (EndPoint endPoint in _clientsEndPoints)
                _socket.SendTo(lastMessage, endPoint);
        }
    }
}