using System.Net;

namespace WPF_project.Data.Models.Interfaces
{
    public abstract class Client
    {
        public static readonly byte[] ConnectionRequestBytesData = { 1 };
        public static readonly byte[] DisconnectNotificationBytesData = { 0 };
        protected bool _isConnected = false;
        protected CancellationTokenSource _source = null!;
        /// <summary>
        /// Raise when client received data
        /// </summary>
        public abstract event EventHandler<byte[]>? DataReceived;
        /// <summary>
        /// Raise when connection with remote host lost
        /// </summary>
        public abstract event EventHandler? ConnectionWithServerLost;
        /// <summary>
        /// Return <c>true</c> if client is connected to remote host;
        /// <c>false</c> - otherwise
        /// </summary>
        public bool IsConnected => _isConnected;
        /// <summary>
        /// Connect to remote host
        /// </summary>
        /// <param name="endPoint">Remote host IP-endpoint</param>
        /// <param name="connectionTime">Time given for connection</param>
        /// <returns><c>true</c> if connection was successful; <c>false</c> - otherwise</returns>
        public abstract bool Connect(IPEndPoint endPoint, TimeSpan connectionTime);
        /// <summary>
        /// Disconnect from connected host
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Send data as bytes to remote host
        /// </summary>
        /// <param name="bytes">Sending data</param>
        public abstract void SendData(byte[] bytes);
    }
}
