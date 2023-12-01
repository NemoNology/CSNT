using System.Net;
using System.Net.Sockets;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Models.Implementations
{
    public class ServerUDP : IServer
    {
        protected UdpClient client = null!;
        public IPEndPoint IPEndPoint { get; set; }
        public CancellationTokenSource Source { get; private set; } = null!;

        public event EventHandler<byte[]>? OnMessageReceived;
        public bool IsRunning => Source is not null
                && !Source.IsCancellationRequested;

        public ServerUDP(IPEndPoint endPoint = null!)
        {
            IPEndPoint = endPoint;
        }

        public void Start()
        {
            if (IsRunning)
                return;

            Source = new();
            client?.Close();
            client = new();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.Connect(IPEndPoint);
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        Source.Token.ThrowIfCancellationRequested();
                        byte[] bytes = (await client.ReceiveAsync(Source.Token)).Buffer;
                        OnMessageReceived?.Invoke(this, bytes);
                    }
                }
                catch
                {
                    return;
                }
            }, Source.Token);
        }

        public void Stop()
        {
            if (IsRunning)
                Source.Cancel();
        }

        public override string ToString()
        {
            return $"Server UDP";
        }
    }
}
