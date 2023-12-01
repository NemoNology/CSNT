using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Models.Implementations
{
    public class ClientUDP : IClient
    {
        readonly UdpClient client = new();

        public IPEndPoint IPEndPoint { get; set; } = null!;
        public bool IsConnected => client.Client.Connected;

        public ClientUDP(IPEndPoint endPoint = null!)
        {
            IPEndPoint = endPoint;
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public void Connect()
        {
            client.Connect(IPEndPoint);
        }

        public void Disconnect()
        {
            client.Close();
        }

        public void SendBytes(byte[] bytes)
        {
            if (IsConnected)
                client.Send(bytes, bytes.Length);
        }

        public override string ToString()
        {
            return $"Client UDP";
        }
    }
}
