using System.Net;

namespace WPF_project.Data.Models.Interfaces
{
    public abstract class Server
    {
        public static readonly byte[] ConnectionAcceptingBytesData = { 1 };
        public static readonly byte[] ShutdownNotificationBytesData = { 0 };
        protected bool _isRunning = false;
        protected CancellationTokenSource _source = null!;
        /// <summary>
        /// Raise when server received data
        /// </summary>
        public abstract event EventHandler<byte[]>? DataReceived;
        /// <summary>
        /// Return <c>true</c> if server is running (listening);
        /// <c>false</c> - otherwise
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Run listening on local host
        /// </summary>
        /// <param name="endPoint">Local host IP-endpoint</param>
        public abstract void Start(IPEndPoint endPoint);

        /// <summary>
        /// Stop server work
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Send data to every listener (connected client)
        /// </summary>
        /// <param name="data">Sending data</param>
        protected abstract void SendData(byte[] data);

        /// <summary>
        /// Send data to specified remote host (client)
        /// </summary>
        /// <param name="data">Sending data</param>
        /// <param name="remoteHost">Remote host that will be received data</param>
        protected abstract void SendData(byte[] data, IPEndPoint remoteHost);
    }
}
