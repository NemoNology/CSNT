using System.Net;
using System.Net.Sockets;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Models.Implementations
{
    public class ClientUDP : IClient
    {
        private bool _isConnected = false;
        private Socket socket = null!;
        private CancellationTokenSource source = null!;
        public event EventHandler<byte[]>? DataReceived;
        public event EventHandler? ConnectionWithServerLost;
        public bool IsConnected => _isConnected;

        void SocketReset()
        {
            if (socket is null || !_isConnected)
            {
                socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
        }

        public void Connect(IPEndPoint endPoint)
        {
            if (_isConnected)
                return;

            SocketReset();
            source = new();
            socket.Connect(endPoint);
            Task.Run(async () =>
            {
                ArraySegment<byte> bytesBuffer = new byte[4096];
                int receivedBytesSize;
                SendBytes(IClient.ConnectBytesData);

                try
                {
                    _isConnected = true;
                    while (_isConnected)
                    {
                        receivedBytesSize = await socket.ReceiveAsync(
                                bytesBuffer,
                                SocketFlags.None,
                                source.Token);

                        if (bytesBuffer == IServer.StopBytesData)
                        {
                            ConnectionWithServerLost?.Invoke(this, EventArgs.Empty);
                            return;
                        }

                        DataReceived?.Invoke(this, bytesBuffer.ToArray()[..receivedBytesSize]);
                    }
                }
                catch
                {
                    _isConnected = false;
                }
            }, source.Token);
        }

        public void Disconnect()
        {
            if (_isConnected)
            {
                _isConnected = false;
                SendBytes(IClient.DisconnectBytesData);
                source.Cancel();
                socket.Close();
            }
        }

        public void SendBytes(byte[] bytes)
        {
            if (_isConnected)
                socket.Send(bytes);
        }

        public override string ToString()
        {
            return "Client UDP";
        }
    }
}
