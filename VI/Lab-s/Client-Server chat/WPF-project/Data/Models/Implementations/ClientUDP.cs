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

        ~ClientUDP()
        {
            Disconnect();
        }

        public override async Task<bool> Connect(
            IPEndPoint clientIPEndPoint,
            IPEndPoint serverIPEndPoint,
            CancellationToken waitingConnectionCancellationToken)
        {
            if (_isConnected)
                return false;

            _source = new();
            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                _socket.Bind(clientIPEndPoint);
                _socket.Connect(serverIPEndPoint);
            }
            catch
            {
                _socket.Close();
                return false;
            }
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
                            try
                            {
                                _socket.Receive(dataBuffer, neededDataLength, SocketFlags.None);
                            }
                            catch
                            {
                                _socket.Close();
                                return false;
                            }

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

                            if (Enumerable.SequenceEqual(bytesBuffer[..receivedBytesSize], Server.ShutdownNotificationBytesData))
                            {
                                ConnectionWithServerLost?.Invoke(this, EventArgs.Empty);
                                Disconnect();
                            }

                            DataReceived?.Invoke(this, bytesBuffer[..receivedBytesSize]);
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
