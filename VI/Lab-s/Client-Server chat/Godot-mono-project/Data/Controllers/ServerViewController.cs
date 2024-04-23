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
            bool isUdp = ProtocolInput.Selected == 0;
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
            else if (!NetHelper.IsAddressForTransportProtocolAvailable(new IPEndPoint(ipAddress, port), isUdp))
            {
                ErrorsOutput.Text = "Данный порт занят";
                return;
            }

            ErrorsOutput.Text = string.Empty;
            _server = isUdp ? new ServerUdp() : new ServerTcp();
            _server.MessageReceived += OnMessageRecieved;

            _server.Start(ipAddress, port);
            CallDeferred(nameof(SwitchControlsVisibility));
        }

        private void OnStopServerButtonPressed()
        {
            _server.Stop();
            _server.MessageReceived -= OnMessageRecieved;
            CallDeferred(nameof(ClearMessages));
            CallDeferred(nameof(SwitchControlsVisibility));
        }

        private void SwitchControlsVisibility()
        {
            ServerRunningControl.Visible = !ServerRunningControl.Visible;
            ServerNotRunningControl.Visible = !ServerNotRunningControl.Visible;
        }

        private void ClearMessages()
        {
            foreach (Node child in MessagesContainer.GetChildren())
                MessagesContainer.RemoveChild(child);
        }

        private void OnMessageRecieved(byte[] messageBytes)
        {
            CallDeferred(nameof(AddMessageAsChild), messageBytes);
        }

        private void AddMessageAsChild(byte[] messageBytes)
        {
            MessagesContainer.AddChild(new Label { Text = Encoding.UTF8.GetString(messageBytes) });
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            _server?.Stop();
        }
    }
}
