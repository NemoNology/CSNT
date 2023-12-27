using System.Net;
using System.Net.Sockets;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Models.Implementations
{
    public class ClientUDP : Client
    {
        public override event EventHandler<byte[]>? DataReceived;
        public override event EventHandler? ConnectionWithServerLost;
        private Socket _socket = null!;

        public override async Task<bool> Connect(IPEndPoint endPoint, CancellationToken waitingConnectionCancellationToken)
        {
            if (_isConnected)
                return false;

            _source = new();
            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Connect(endPoint);
            _isConnected = true;
            // Check server answer
            _isConnected = await Task.Run(
                () =>
                {
                    SendData(ConnectionRequestBytesData);
                    int neededDataLength = Server.ConnectionAcceptingBytesData.Length;
                    byte[] dataBuffer = new byte[neededDataLength];

                    while (!waitingConnectionCancellationToken.IsCancellationRequested)
                    {
                        if (_socket.Available >= neededDataLength)
                        {
                            _socket.Receive(dataBuffer, neededDataLength, SocketFlags.None);
                            if (Enumerable.SequenceEqual(dataBuffer, Server.ConnectionAcceptingBytesData))
                                return true;
                        }
                    }

                    return false;
                });

            // If server accept request -> Start listen server
            if (_isConnected)
            {
                _ = Task.Run(() =>
                {
                    byte[] bytesBuffer = new byte[4096];
                    int receivedBytesSize;

                    while (!_source.IsCancellationRequested)
                    {
                        if (_socket.Available > 0)
                        {
                            receivedBytesSize = _socket.Receive(bytesBuffer);

                            if (Enumerable.SequenceEqual(bytesBuffer, Server.ShutdownNotificationBytesData))
                            {
                                ConnectionWithServerLost?.Invoke(this, EventArgs.Empty);
                                Disconnect();
                            }

                            DataReceived?.Invoke(this, bytesBuffer.ToArray()[..receivedBytesSize]);
                        }
                    }
                });
            }

            return _isConnected;
        }

        public override void Disconnect()
        {
            if (_isConnected)
            {
                SendData(DisconnectNotificationBytesData);
                _isConnected = false;
                _source.Cancel();
                _socket.Close();
            }
        }

        public override void SendData(byte[] bytes)
        {
            if (_isConnected)
                _socket.Send(bytes);
        }

        public override string ToString()
        {
            return "Client UDP";
        }
    }
}
