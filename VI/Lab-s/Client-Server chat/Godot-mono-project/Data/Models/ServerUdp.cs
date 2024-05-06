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
        private readonly UdpClient _server;
        private readonly List<IPEndPoint> _clientsIpEndPoints = new(2);

        public override event Action<byte[]> MessageReceived;

        public ServerUdp()
        {
            _server = new(AddressFamily.InterNetwork);
        }

        public override void Start(IPAddress ipAddress, int port)
        {
            if (_isRunning)
                return;

            _server.Client.Bind(new IPEndPoint(ipAddress, port));
            _isRunning = true;
            // Add init massage to messages
            _messagesBytes.Add(Encoding.UTF8.GetBytes(
                GetStartRunningMessage(_server.Client.LocalEndPoint)));
            SendLastMessageToClients();
            // Thread for receiving messages
            Task.Run(() =>
            {
                while (_isRunning)
                {
                    IPEndPoint clientIpEndPoint = new(IPAddress.Any, 0);
                    var buffer = _server.Receive(ref clientIpEndPoint);
                    bool isNewClient = !_clientsIpEndPoints.Contains(clientIpEndPoint);

                    // Check if it's new client
                    if (isNewClient)
                    {
                        // Send new client all messages
                        foreach (byte[] msg in _messagesBytes)
                            _server.Send(msg, clientIpEndPoint);
                        // Add new client
                        _clientsIpEndPoints.Add(clientIpEndPoint);
                        lock (_messagesBytes)
                        {
                            _messagesBytes.Add(
                                Encoding.UTF8.GetBytes(
                                    GetClientConnectedMessage(clientIpEndPoint)));
                        }
                        // Notify clients about new client connection
                        SendLastMessageToClients();
                    }

                    lock (_messagesBytes)
                    {
                        if (!isNewClient && Enumerable.SequenceEqual(buffer, NetHelper.SpecialMessageBytes))
                        {
                            _messagesBytes.Add(Encoding.UTF8.GetBytes(
                                GetClientDisconnectedMessage(clientIpEndPoint)));

                            // Remove disconnected client
                            lock (_clientsIpEndPoints)
                                _clientsIpEndPoints.Remove(clientIpEndPoint);
                        }
                        else
                        {
                            _messagesBytes.Add(
                                GetClientFormattedMessageAsBytes(clientIpEndPoint, buffer));
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
            if (!_isRunning)
                return;

            var lastMessage = _messagesBytes[^1];
            MessageReceived?.Invoke(lastMessage);
            foreach (IPEndPoint endPoint in _clientsIpEndPoints)
                _server.Send(lastMessage, endPoint);
        }
    }
}