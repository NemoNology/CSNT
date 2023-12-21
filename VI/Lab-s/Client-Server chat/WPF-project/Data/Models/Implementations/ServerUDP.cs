using System.Net;
using System.Net.Sockets;
using System.Text;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Models.Implementations
{
    public class ServerUDP : IServer
    {
        private bool _isRunning = false;
        private readonly List<IPEndPoint> clientsIPEndPoints = new();
        private UdpClient client = null!;
        private CancellationTokenSource source = null!;
        public event EventHandler<byte[]>? DataReceived;
        public bool IsRunning => _isRunning;

        void SocketReset(IPEndPoint endPoint)
        {
            if (client is null || !_isRunning)
            {
                client = new(endPoint);
            }
        }

        public void Start(IPEndPoint endPoint)
        {
            if (_isRunning)
                return;

            source = new();
            SocketReset(endPoint);
            Task.Run(async () =>
            {
                UdpReceiveResult receivedResultBuffer;

                try
                {
                    _isRunning = true;
                    while (_isRunning)
                    {
                        receivedResultBuffer = await client.ReceiveAsync(source.Token);

                        if (receivedResultBuffer.Buffer == IClient.ConnectBytesData
                            && !clientsIPEndPoints.Contains(receivedResultBuffer.RemoteEndPoint))
                        {
                            clientsIPEndPoints.Add(receivedResultBuffer.RemoteEndPoint);
                            byte[] bytes = Encoding.UTF8.GetBytes($"{receivedResultBuffer.RemoteEndPoint} connected; {DateTime.Now}");
                            SendBytes(bytes);
                            DataReceived?.Invoke(this, bytes);
                            continue;
                        }
                        else if (receivedResultBuffer.Buffer == IClient.DisconnectBytesData)
                        {
                            clientsIPEndPoints.Remove(receivedResultBuffer.RemoteEndPoint);
                            byte[] bytes = Encoding.UTF8.GetBytes($"{receivedResultBuffer.RemoteEndPoint} disconnected; {DateTime.Now}");
                            SendBytes(bytes);
                            DataReceived?.Invoke(this, bytes);
                            continue;
                        }

                        SendBytes(receivedResultBuffer.Buffer);
                        DataReceived?.Invoke(this, receivedResultBuffer.Buffer);
                    }
                }
                catch
                {
                    _isRunning = false;
                    SendBytes(IServer.StopBytesData);
                }
            }, source.Token);
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                source.Cancel();
                client.Close();
                clientsIPEndPoints.Clear();
            }
        }

        public void SendBytes(byte[] data)
        {
            if (_isRunning && clientsIPEndPoints.Count > 0)
            {
                foreach (IPEndPoint remoteEndPoint in clientsIPEndPoints)
                {
                    client.Client.SendTo(data, remoteEndPoint);
                }
            }
        }

        public override string ToString()
        {
            return $"Server UDP";
        }
    }
}
