using System;
using System.Linq;
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
        public Label MessagesOutput { get; set; }

        private void OnRunServerButtonPressed()
        {
            ushort port;
            bool isUdp = ProtocolInput.Selected == 0;
            if (!IPAddress.TryParse(IpAddressInput.Text, out IPAddress ipAddress))
            {
                PrintErrorMessage("Неверный IP-адрес");
                return;
            }
            else if (!ushort.TryParse(PortInput.Text, out port))
            {
                PrintErrorMessage("Неверный порт");
                return;
            }
            else if (!NetHelper.IsAddressForTransportProtocolAvailable(new IPEndPoint(ipAddress, port), isUdp))
            {
                PrintErrorMessage("Данный адрес занят");
                return;
            }

            PrintErrorMessage("");
            _server = isUdp ? new ServerUdp() : new ServerTcp();
            _server.MessageReceived += OnMessageReceivedDeferred;
            _server.StateChanged += OnServerStateChangedDeferred;

            try
            {
                _server.Start(ipAddress, port);
            }
            catch (Exception e)
            {
                PrintErrorMessageDeferred("Не удалось запустить сервер:\n" + e.Message);
            }
        }

        private void PrintErrorMessageDeferred(string v)
        {
            CallDeferred(nameof(PrintErrorMessage), v);
        }

        private void OnServerStateChangedDeferred(bool isRunning)
        {
            CallDeferred(nameof(OnServerStateChanged), isRunning);
        }

        private void OnMessageReceivedDeferred(byte[] messageBytes)
        {
            CallDeferred(nameof(AddMessageToOutput), messageBytes);
        }

        private void OnServerStateChanged(bool state)
        {
            switch (state)
            {
                case true:
                    ServerRunningControl.Visible = true;
                    ServerNotRunningControl.Visible = false;
                    return;
                case false:
                    ServerRunningControl.Visible = false;
                    ServerNotRunningControl.Visible = true;
                    return;
            }
        }

        private void OnStopServerButtonPressed()
        {
            _server.Stop();
            _server.MessageReceived -= OnMessageReceivedDeferred;
            _server.StateChanged -= OnServerStateChangedDeferred;
            CallDeferred(nameof(ClearMessages));
        }

        private void ClearMessages()
        {
            MessagesOutput.Text = "";
        }

        private void AddMessageToOutput(byte[] messageBytes)
        {
            MessagesOutput.Text += Encoding.UTF8.GetString(messageBytes);
        }

        private void PrintErrorMessage(string message)
        {
            ErrorsOutput.Text = message;
        }

        public override void _ExitTree()
        {
            if (_server is not null)
                OnStopServerButtonPressed();
            base._ExitTree();
        }
    }
}
