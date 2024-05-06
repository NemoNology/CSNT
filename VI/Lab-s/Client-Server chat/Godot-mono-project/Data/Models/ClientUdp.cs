using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ClientUdp : Client
    {
        private readonly UdpClient _client;

        public override event Action<byte[]> MessageReceived;

        public ClientUdp()
        {
            _client = new(AddressFamily.InterNetwork);
        }

        public override void Connect(IPAddress clientIpAddress, int clientPort, IPAddress serverIpAddress, int serverPort)
        {
            if (_isConnected)
                return;

            var serverEndpoint = new IPEndPoint(serverIpAddress, serverPort);
            _client.Client.Bind(new IPEndPoint(clientIpAddress, clientPort));
            _client.Connect(serverEndpoint);
            _isConnected = true;
            // Thread for recieving messages
            Task.Run(() =>
            {
                // Send message to server which means that client connected
                SendMessage();
                while (_isConnected)
                {
                    var buffer = _client.Receive(ref serverEndpoint);
                    MessageReceived?.Invoke(buffer);
                    // If special message recieved => server is closing, client needs to disconnect
                    if (Enumerable.SequenceEqual(buffer, NetHelper.SpecialMessageBytes))
                    {
                        Disconnect();
                        return;
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        public override void Disconnect()
        {
            if (!_isConnected)
                return;

            SendMessage();
            _isConnected = false;
            _cancellationTokenSource.Cancel();
        }

        public override void SendMessage(string message = "")
        {
            if (!_isConnected)
                return;

            _client.Send(
                message == "" ? NetHelper.SpecialMessageBytes : Encoding.UTF8.GetBytes(message));
        }
    }
}