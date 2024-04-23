using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ClientUdp : Client
    {
        public override event Action<byte[]> MessageReceived;

        public ClientUdp()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public override void Connect(IPAddress clientIpAddress, int clientPort, IPAddress serverIpAddress, int serverPort)
        {
            if (_isConnected || _isDisposed)
                return;

            _socket.Bind(new IPEndPoint(clientIpAddress, clientPort));
            _socket.Connect(new IPEndPoint(serverIpAddress, serverPort));
            _isConnected = true;
            Task.Run(() =>
            {
                SendMessage();
                byte[] buffer = new byte[4096];
                while (_isConnected)
                {
                    var recievedBytesLength = _socket.Receive(buffer);
                    MessageReceived?.Invoke(buffer[..recievedBytesLength]);
                    if (recievedBytesLength == 0)
                    {
                        Disconnect();
                        return;
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        public override void Disconnect()
        {
            if (!_isConnected || _isDisposed)
                return;

            SendMessage();
            _isConnected = false;
            _cancellationTokenSource.Cancel();
        }

        public override void SendMessage(string message = "")
        {
            if (!_isConnected || _isDisposed)
                return;

            _socket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}