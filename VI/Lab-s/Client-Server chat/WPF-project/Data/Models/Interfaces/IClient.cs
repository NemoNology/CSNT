using System.Net;

namespace WPF_project.Data.Models.Interfaces
{
    public interface IClient
    {
        public IPEndPoint IPEndPoint { get; set; }
        public bool IsConnected { get; }
        public void Connect();
        public void Disconnect();
        public void SendBytes(byte[] bytes);
    }
}
