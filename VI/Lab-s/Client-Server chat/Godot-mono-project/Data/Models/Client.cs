using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSNT.Clientserverchat.Data.Models
{
    public abstract class Client
    {
        protected Socket _socket;
        protected ClientState _state = ClientState.Disconnected;
        protected readonly CancellationTokenSource _cancellationTokenSource = new();

        public abstract event Action<byte[]> MessageReceived;
        public event Action<ClientState> StateChanged;

        public ClientState State
        {
            get => _state;
            protected set
            {
                _state = value;
                StateChanged?.Invoke(_state);
            }
        }

        public abstract void Connect(IPAddress clientIpAddress, int clientPort, IPAddress serverIpAddress, int serverPort);
        /// <summary>
        /// Disconnects from server
        /// </summary>
        /// <param name="isForced">
        /// <c>true</c> if server closing, so client is forced to disconnect, <c>false</c> if client disconnecting by himself
        /// </param>
        public abstract void Disconnect(bool isForced = false);
        public abstract void SendMessage(string message);
    }
}