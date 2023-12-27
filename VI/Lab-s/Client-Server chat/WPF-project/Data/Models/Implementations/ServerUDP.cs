using System.Net;
using System.Net.Sockets;
using System.Text;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Models.Implementations
{
    public class ServerUDP : Server
    {
        public override event EventHandler<byte[]>? DataReceived;
        private UdpClient _client = null!;
        /// <summary>
        /// List of listeners - connected clients
        /// </summary>
        private readonly List<IPEndPoint> _clientsIPEndPoints = new(2);

        public override void Start(IPEndPoint endPoint)
        {
            if (_isRunning)
                return;

            _source = new();
            _client = new(endPoint);
            Task.Run(async () =>
            {
                try
                {
                    UdpReceiveResult resultBuffer;

                    _isRunning = true;
                    while (_isRunning)
                    {
                        resultBuffer = await _client.ReceiveAsync(_source.Token);
                        bool isClientConnectedAlready =
                            _clientsIPEndPoints.Contains(resultBuffer.RemoteEndPoint);
                        bool isConnectionRequest =
                            Enumerable.SequenceEqual(resultBuffer.Buffer, Client.ConnectionRequestBytesData);

                        if (!(isConnectionRequest || isClientConnectedAlready))
                            continue;

                        if (isConnectionRequest)
                        {
                            SendData(ConnectionAcceptingBytesData, resultBuffer.RemoteEndPoint);
                            if (!isClientConnectedAlready)
                            {
                                byte[] bytes = Encoding.UTF8.GetBytes($"{resultBuffer.RemoteEndPoint} connected");
                                SendData(bytes);
                                DataReceived?.Invoke(this, bytes);
                                _clientsIPEndPoints.Add(resultBuffer.RemoteEndPoint);
                            }
                        }
                        else if (Enumerable.SequenceEqual(resultBuffer.Buffer, Client.DisconnectNotificationBytesData))
                        {
                            if (isClientConnectedAlready)
                            {
                                _clientsIPEndPoints.Remove(resultBuffer.RemoteEndPoint);
                                byte[] bytes = Encoding.UTF8.GetBytes($"{resultBuffer.RemoteEndPoint} disconnected");
                                SendData(bytes);
                                DataReceived?.Invoke(this, bytes);
                            }
                        }
                        else
                        {
                            SendData(resultBuffer.Buffer);
                            DataReceived?.Invoke(this, resultBuffer.Buffer);
                        }
                    }
                }
                finally
                {
                    Stop();
                }
            }, _source.Token);
        }

        public override void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                SendData(ShutdownNotificationBytesData);
                _source.Cancel();
                _client.Close();
                _clientsIPEndPoints.Clear();
            }
        }

        protected override void SendData(byte[] data)
        {
            if (_isRunning)
            {
                foreach (IPEndPoint remoteHost in _clientsIPEndPoints)
                {
                    _client.Client.SendTo(data, remoteHost);
                }
            }
        }

        protected override void SendData(byte[] data, IPEndPoint remoteHost)
        {
            if (_isRunning)
            {
                _client.Client.SendTo(data, remoteHost);
            }
        }

        public override string ToString()
        {
            return "Server UDP";
        }
    }
}
