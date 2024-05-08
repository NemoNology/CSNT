using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        /// <summary>
        /// <b>State</b>: <c>true</c> if server running, <c>false</c> - otherwise
        /// </summary>
        public event Action<bool> StateChanged;
        /// <summary>
        /// <c>true</c> if server running, <c>false</c> - otherwise
        /// </summary>
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                StateChanged?.Invoke(_isRunning);
            }
        }

        public abstract void Start(IPAddress ipAddress, int port);

        public abstract void Stop();

        public static string GetInitializeMessage(EndPoint localEndPoint)
            => $"Сервер ({localEndPoint}) ({DateTime.Now}) запущен";

        public static string GetClientConnectedMessage(EndPoint clientEndPoint)
         => $"Клиент ({clientEndPoint}) ({DateTime.Now}) подключился";

        public static string GetClientDisconnectedMessage(EndPoint clientEndPoint)
        => $"Клиент ({clientEndPoint}) ({DateTime.Now}) отключился";

        public static byte[] GetClientFormattedMessageAsBytes(EndPoint clientEndPoint, byte[] messageBytes)
         => Encoding.UTF8.GetBytes($"{clientEndPoint} ({DateTime.Now}): ").Concat(messageBytes).ToArray();
    }
}