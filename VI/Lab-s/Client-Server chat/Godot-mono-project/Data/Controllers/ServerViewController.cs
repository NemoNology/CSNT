using System.Net;
using System.Text;
using CSNT.Clientserverchat.Data.Models;
using Godot;

namespace CSNT.Clientserverchat.Data.Controllers
{
    public partial class ServerViewController : Control
    {
        private Server _server;

        [ExportCategory("Server view controller")]
        [Export]
        public Control ServerNotRunningControl { get; set; }
        [Export]
        public Control ServerRunningControl { get; set; }
        [Export]
        public LineEdit IpAddressInput { get; set; }
        [Export]
        public LineEdit PortInput { get; set; }
        [Export]
        public OptionButton ProtocolInput { get; set; }
        [Export]
        public Label ErrorsOutput { get; set; }
        [Export]
        public VBoxContainer MessagesContainer { get; set; }


        private void OnRunServerButtonPressed()
        {
            ushort port;
            int protocolType = ProtocolInput.Selected;
            if (!IPAddress.TryParse(IpAddressInput.Text, out IPAddress ipAddress))
            {
                ErrorsOutput.Text = "Неверный IP-адрес";
                return;
            }
            else if (!ushort.TryParse(PortInput.Text, out port))
            {
                ErrorsOutput.Text = "Неверный порт";
                return;
            }
            else if (!NetHelper.IsAddressForTransportProtocolAvailable(new IPEndPoint(ipAddress, port), protocolType == 0))
            {
                ErrorsOutput.Text = "Данный порт занят";
                return;
            }

            _server = protocolType == 0 ? new ServerUdp() : new ServerTcp();
            _server.MessageReceived += AddMessage;

            _server.Start(ipAddress, port);
            ServerRunningControl.Visible = true;
            ServerNotRunningControl.Visible = false;
        }

        private void OnStopServerButtonPressed()
        {
            _server.Stop();
            _server.MessageReceived -= AddMessage;
            foreach (Node child in MessagesContainer.GetChildren())
                MessagesContainer.RemoveChild(child);
            ServerRunningControl.Visible = false;
            ServerNotRunningControl.Visible = true;
        }

        private void AddMessage(byte[] messageBytes)
        {
            MessagesContainer.AddChild(new Label { Text = Encoding.UTF8.GetString(messageBytes) });
        }
    }
}
