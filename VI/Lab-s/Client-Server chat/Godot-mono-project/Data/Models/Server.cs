using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSNT.Clientserverchat.Data.Models
{
    public abstract class Server : IDisposable
    {
        protected Socket _socket;
        protected bool _isRunning = false;
        protected bool _isDisposed = false;
        protected readonly CancellationTokenSource _cancellationTokenSource = new();
        protected readonly List<byte[]> _messagesBytes = new(16);

        public abstract event Action<byte[]> MessageReceived;
        public bool IsRunning => _isRunning;

        public abstract void Start(IPAddress ipAddress, int port);

        public abstract void Stop();

        public void Dispose()
        {
            if (_isDisposed)
                return;

            Stop();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Dispose();
            _cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
            _isDisposed = true;
        }
    }
}