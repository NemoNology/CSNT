using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ClientTcp : Client
    {
        // private Socket _connectedSocket;
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
            _isConnected = true;
            // Thread for connection and recieving messages
            Task.Run(async () =>
            {
                byte[] buffer = new byte[4096];
                // Start listening (Awaiting connection) and accept connection
                // _socket.Listen(1);
                // _connectedSocket = _socket.Accept();
                // Start recieve messages
                while (_isConnected)
                {
                    // var recievedBytesLength = _connectedSocket.Receive(buffer);
                    var recievedBytesLength = await _socket.ReceiveAsync(
                        buffer, SocketFlags.None, _cancellationTokenSource.Token);
                    byte[] recievedBytes = buffer[..recievedBytesLength];
                    MessageReceived?.Invoke(recievedBytes);
                    // If message is empty - server clising, so we need to disconnect
                    if (Enumerable.SequenceEqual(recievedBytes, ServerTcp.CloseMessageBytes))
                    {
                        Disconnect();
                        return;
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        public override async void Disconnect()
        {
            if (!_isConnected || _isDisposed)
                return;

            _isConnected = false;
            await _socket.DisconnectAsync(true);
            _cancellationTokenSource.Cancel();
        }

        public override void SendMessage(string message)
        {
            if (!_isConnected || _isDisposed || !_socket.Connected)
                return;

            // _connectedSocket.Send(Encoding.UTF8.GetBytes(message));
            _socket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}