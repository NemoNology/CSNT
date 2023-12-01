using System.Net;

namespace WPF_project.Data.Models.Interfaces
{
    public interface IServer
    {
        public IPEndPoint IPEndPoint { get; set; }
        public CancellationTokenSource Source { get; }
        public bool IsRunning { get; }
        public void Start();
        public void Stop();
        public event EventHandler<byte[]>? OnMessageReceived;
    }
}
