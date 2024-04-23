using System;
using System.Net;
using System.Net.Sockets;

namespace CSNT.Clientserverchat.Data.Models
{
    public abstract class Client
    {
        protected Socket _socket;

        public abstract event Action<byte[]> MessageReceived;

        public abstract void Connect(IPAddress ipAddress, int port);
        public abstract void Dissconnect();
        public abstract void SendMessage(string message);
    }
}