using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSNT.Clientserverchat.Data.Models
{
    public abstract class Server
    {
        protected Socket _socket;
        protected bool _isRunning = false;
        protected readonly CancellationTokenSource _cancellationTokenSource = new();
        protected readonly List<byte[]> _messagesBytes = new(16);

        public abstract event Action<byte[]> MessageReceived;
        public bool IsRunning => _isRunning;

        public abstract void Start(IPAddress ipAddress, int port);

        public abstract void Stop();
    }
}