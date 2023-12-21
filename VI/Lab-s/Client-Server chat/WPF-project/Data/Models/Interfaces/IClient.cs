using System.Net;

namespace WPF_project.Data.Models.Interfaces
{
    public interface IClient
    {
        public static readonly byte[] DisconnectBytesData = Array.Empty<byte>();
        public static readonly byte[] ConnectBytesData = { 1 };
        /// <summary>
        /// Raise when remote host received data
        /// </summary>
        public event EventHandler<byte[]>? DataReceived;
        /// <summary>
        /// Raise when connection with remote host lost
        /// </summary>
        public event EventHandler? ConnectionWithServerLost;
        /// <summary>
        /// Return <c>true</c> if client is connected to remote host;
        /// <c>false</c> - otherwise
        /// </summary>
        public bool IsConnected { get; }
        /// <summary>
        /// Connect to remote host
        /// </summary>
        /// <param name="endPoint">Remote host IP-endpoint</param>
        public void Connect(IPEndPoint endPoint);
        /// <summary>
        /// Disconnect from connected host
        /// </summary>
        public void Disconnect();
        /// <summary>
        /// Send data as bytes to remote host
        /// </summary>
        /// <param name="bytes">Sending data</param>
        public void SendBytes(byte[] bytes);
    }
}
