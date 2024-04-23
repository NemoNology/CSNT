using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ClientTcp : Client
    {
        public override event Action<byte[]> MessageReceived;

        public ClientTcp()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }


        public override void Connect(IPAddress clientIpAddress, int clientPort, IPAddress serverIpAddress, int serverPort)
        {
            if (_isConnected)
                return;

            _socket.Bind(new IPEndPoint(clientIpAddress, clientPort));
            _socket.Connect(new IPEndPoint(serverIpAddress, serverPort));
            Task.Run(() =>
            {
                byte[] buffer = new byte[4096];
                while (_isConnected)
                {
                    var recievedBytesLength = _socket.Receive(buffer);
                    MessageReceived?.Invoke(buffer[..recievedBytesLength]);
                }
            }, _cancellationTokenSource.Token);
        }

        public override void Dissconnect()
        {
            if (!_isConnected)
                return;

            _socket.Disconnect(true);
            _isConnected = false;
            _cancellationTokenSource.Cancel();
        }

        public override void SendMessage(string message)
        {
            if (!_isConnected)
                return;

            _socket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}