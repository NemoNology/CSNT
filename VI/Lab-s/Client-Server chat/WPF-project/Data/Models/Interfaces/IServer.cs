using System.Net;
using System.Text;

namespace WPF_project.Data.Models.Interfaces
{
    public interface IServer
    {
        public static readonly byte[] StopBytesData = Array.Empty<byte>();
        /// <summary>
        /// Raise when server received data
        /// </summary>
        public event EventHandler<byte[]>? DataReceived;
        /// <summary>
        /// Return <c>true</c> if server is running (listening);
        /// <c>false</c> - otherwise
        /// </summary>
        public bool IsRunning { get; }

        /// <summary>
        /// Start listening on local host
        /// </summary>
        /// <param name="endPoint">Local host IP-endpoint</param>
        public void Start(IPEndPoint endPoint);
       
        /// <summary>
        /// Stop listening
        /// </summary>
        public void Stop();
    }
}
