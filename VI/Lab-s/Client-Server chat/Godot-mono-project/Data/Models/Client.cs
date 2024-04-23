using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSNT.Clientserverchat.Data.Models
{
    public abstract class Client
    {
        protected Socket _socket;
        protected bool _isConnected = false;
        protected readonly CancellationTokenSource _cancellationTokenSource = new();

        public abstract event Action<byte[]> MessageReceived;

        public bool IsConnected => _isConnected;

        public abstract void Connect(IPAddress clientIpAddress, int clientPort, IPAddress serverIpAddress, int serverPort);
        public abstract void Dissconnect();
        public abstract void SendMessage(string message);
    }
}