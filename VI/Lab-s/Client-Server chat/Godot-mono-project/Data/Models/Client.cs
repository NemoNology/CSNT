using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSNT.Clientserverchat.Data.Models
{
    public abstract class Client : IDisposable
    {
        protected Socket _socket;
        protected bool _isConnected = false;
        protected bool _isDisposed = false;
        protected readonly CancellationTokenSource _cancellationTokenSource = new();

        public abstract event Action<byte[]> MessageReceived;

        public bool IsConnected => _isConnected;

        public abstract void Connect(IPAddress clientIpAddress, int clientPort, IPAddress serverIpAddress, int serverPort);
        public abstract void Disconnect();
        public abstract void SendMessage(string message);

        public void Dispose()
        {
            if (_isDisposed)
                return;

            Disconnect();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Dispose();
            _cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
            _isDisposed = true;
        }
    }
}