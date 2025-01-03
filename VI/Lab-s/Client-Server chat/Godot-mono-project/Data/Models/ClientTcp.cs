using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ClientTcp : Client
    {
        public override event Action<byte[]> MessageReceived;

        public ClientTcp()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public override void Connect(IPAddress clientIpAddress, int clientPort, IPAddress serverIpAddress, int serverPort)
        {
            if (_state != ClientState.Disconnected)
                return;

            _socket.Bind(new IPEndPoint(clientIpAddress, clientPort));
            // Connecting to server
            State = ClientState.Connecting;
            _socket.Connect(new IPEndPoint(serverIpAddress, serverPort));
            State = ClientState.Connected;
            // Thread for connection and receiving messages
            Task.Run(() =>
            {
                byte[] buffer = new byte[4096];
                // Start receive messages
                while (_state == ClientState.Connected)
                {
                    int receivedBytesLength = _socket.Receive(buffer);
                    byte[] receivedBytes = buffer[..receivedBytesLength];
                    MessageReceived?.Invoke(receivedBytes);
                    // If message is special - server closing, so client needs to disconnect
                    if (Enumerable.SequenceEqual(receivedBytes, NetHelper.SpecialMessageBytes))
                    {
                        Disconnect(true);
                        return;
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        public override void Disconnect(bool isForced = false)
        {
            if (_state == ClientState.Disconnected)
                return;

            SendMessage();
            _cancellationTokenSource.CancelAfter(1000);
            Task.Run(async () =>
            {
                if (_socket.Connected)
                {
                    await _socket.DisconnectAsync(true);
                }
            }, _cancellationTokenSource.Token);
            State = ClientState.Disconnected;
        }

        public override void SendMessage(string message = "")
        {
            if (_state != ClientState.Connected || !_socket.Connected)
                return;

            _socket.Send(
                message == "" ? NetHelper.SpecialMessageBytes : Encoding.UTF8.GetBytes(message)
            );
        }
    }
}