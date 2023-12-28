using System.Net;
using System.Net.Sockets;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Models.Implementations
{
    public class ClientTCP : Client
    {
        public override event EventHandler<byte[]>? DataReceived;
        public override event EventHandler? ConnectionWithServerLost;
        private TcpClient _client = null!;
        private NetworkStream _clientStream = null!;

        ~ClientTCP()
        {
            Disconnect();
        }

        public override async Task<bool> Connect(
            IPEndPoint clientIPEndPoint,
            IPEndPoint serverIPEndPoint,
            CancellationToken waitingConnectionCancellationToken)
        {
            if (_isConnected)
                return false;

            _source = new();
            _client = new(clientIPEndPoint);
            _isConnected = true;
            // Check server answer
            _isConnected = await Task.Run(
                () =>
                {
                    try
                    {
                        _client.Connect(serverIPEndPoint);
                    }
                    catch
                    {
                        return false;
                    }

                    return true;
                }, _source.Token);

            // If server accept request -> Start listen server
            if (_isConnected)
            {
                _ = Task.Run(() =>
                {
                    Span<byte> buffer = new byte[4096];
                    int receivedBytesSize;
                    byte[] bytesBuffer;

                    _clientStream = _client.GetStream();

                    while (_clientStream.CanRead && !_source.IsCancellationRequested)
                    {
                        if (_clientStream.DataAvailable)
                        {
                            receivedBytesSize = _clientStream.Read(buffer);
                            bytesBuffer = buffer[..receivedBytesSize].ToArray();

                            if (Enumerable.SequenceEqual(bytesBuffer, DisconnectNotificationBytesData))
                            {
                                ConnectionWithServerLost?.Invoke(this, EventArgs.Empty);
                                break;
                            }

                            DataReceived?.Invoke(this, bytesBuffer);
                        }
                    }

                    Disconnect();
                }, _source.Token);
            }

            return _isConnected;
        }

        public override void Disconnect()
        {
            if (_isConnected)
            {
                SendData(DisconnectNotificationBytesData);
                _isConnected = false;
                _clientStream.Close();
                _source.Cancel();
                _client.Close();
            }
        }

        public override void SendData(byte[] bytes)
        {
            if (_isConnected && _clientStream.CanWrite)
            {
                _clientStream.Write(bytes);
                DataReceived?.Invoke(this, bytes);
            }
        }

        public override string ToString()
        {
            return "Client TCP";
        }
    }
}