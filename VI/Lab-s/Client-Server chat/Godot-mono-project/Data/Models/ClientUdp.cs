using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSNT.Clientserverchat.Data.Models
{
    public class ClientUdp : Client
    {
        public override event Action<byte[]> MessageReceived;

        public ClientUdp()
        {
            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public override void Connect(IPAddress clientIpAddress, int clientPort, IPAddress serverIpAddress, int serverPort)
        {
            if (_state != ClientState.Disconnected)
                return;

            _socket.Bind(new IPEndPoint(clientIpAddress, clientPort));
            _socket.Connect(new IPEndPoint(serverIpAddress, serverPort));
            State = ClientState.Connecting;
            // Thread for recieving messages
            Task.Run(() =>
            {
                byte[] buffer = new byte[NetHelper.BUFFERSIZE];
                // Send message to server which means that client started connecting
                SendMessage();
                // Wait for server answer
                while (_state == ClientState.Connecting)
                {
                    if (_socket.Available > 0)
                    {
                        State = ClientState.Connected;
                        break;
                    }
                }
                // If connected -> start listenning
                while (_state == ClientState.Connected)
                {
                    int recievedBytesLength = _socket.Receive(buffer);
                    byte[] recievedBytes = buffer[..recievedBytesLength];
                    MessageReceived?.Invoke(recievedBytes);
                    // If special message recieved => server is closing, client needs to disconnect
                    if (Enumerable.SequenceEqual(recievedBytes, NetHelper.SpecialMessageBytes))
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

            if (!isForced)
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
            if (_state == ClientState.Disconnected)
                return;

            _socket.Send(
                message == "" ? NetHelper.SpecialMessageBytes : Encoding.UTF8.GetBytes(message));
        }
    }
}