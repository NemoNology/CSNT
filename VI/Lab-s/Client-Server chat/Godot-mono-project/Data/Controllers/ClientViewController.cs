using CSNT.Clientserverchat.Data.Models;
using Godot;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace CSNT.Clientserverchat.Data.Controllers
{
    public partial class ClientViewController : Control
    {
        private Client _client;

        [ExportCategory("Client view controller")]
        [Export]
        public Control ClientDisconnectedControl { get; set; }
        [Export]
        public Control ClientConnectingControl { get; set; }
        [Export]
        public Control ClientConnectedControl { get; set; }
        [Export]
        public LineEdit ClientIpInput { get; set; }
        [Export]
        public LineEdit ClientPortInput { get; set; }
        [Export]
        public LineEdit ServerIpInput { get; set; }
        [Export]
        public LineEdit ServerPortInput { get; set; }
        [Export]
        public OptionButton ProtocolInput { get; set; }
        [Export]
        public Label ErrorsOutput { get; set; }
        [Export]
        public Label MessagesOutput { get; set; }
        [Export]
        public LineEdit MessageInput { get; set; }

        private void OnConnectButtonPressed()
        {
            ushort clientPort, serverPort;
            IPAddress serverIpAddress;
            bool isUdp = ProtocolInput.Selected == 0;
            PrintErrorMessageDeferred("");
            if (!IPAddress.TryParse(ClientIpInput.Text, out IPAddress clientIpAddress))
            {
                PrintErrorMessageDeferred("Неверный IP-адрес клиента");
                return;
            }
            else if (!ushort.TryParse(ClientPortInput.Text, out clientPort))
            {
                PrintErrorMessageDeferred("Неверный порт клиента");
                return;
            }
            else if (!IPAddress.TryParse(ServerIpInput.Text, out serverIpAddress))
            {
                PrintErrorMessageDeferred("Неверный IP-адрес сервера");
                return;
            }
            else if (!ushort.TryParse(ServerPortInput.Text, out serverPort))
            {
                PrintErrorMessageDeferred("Неверный порт сервера");
                return;
            }
            else if (!NetHelper.IsAddressForTransportProtocolAvailable(new IPEndPoint(clientIpAddress, clientPort), isUdp))
            {
                PrintErrorMessageDeferred("Данный адрес для клиента занят");
                return;
            }

            _client = isUdp ? new ClientUdp() : new ClientTcp();
            _client.MessageReceived += OnMessageReceivedDeferred;
            _client.StateChanged += OnClientStateChangedDeferred;

            try
            {
                _client.Connect(clientIpAddress, clientPort, serverIpAddress, serverPort);
            }
            catch (Exception e)
            {
                PrintErrorMessageDeferred("Не удалось подключиться:\n" + e.Message);
                OnDisconnectButtonPressed();
            }
        }

        private void OnClientStateChangedDeferred(ClientState state)
        {
            CallDeferred(nameof(OnClientStateChanged), (int)state);
        }

        private void OnMessageReceivedDeferred(byte[] messageBytes)
        {
            if (Enumerable.SequenceEqual(messageBytes, NetHelper.SpecialMessageBytes))
            {
                OnDisconnectButtonPressed();
                PrintErrorMessageDeferred("Сервер был остановлен");
                return;
            }
            CallDeferred(nameof(AddMessageToOutput), messageBytes);
        }

        private void OnClientStateChanged(int state)
        {
            switch (state)
            {
                case 0:
                    ClientDisconnectedControl.Visible = true;
                    ClientConnectingControl.Visible = false;
                    ClientConnectedControl.Visible = false;
                    return;
                case 1:
                    ClientDisconnectedControl.Visible = false;
                    ClientConnectingControl.Visible = true;
                    ClientConnectedControl.Visible = false;
                    return;
                case 2:
                    ClientDisconnectedControl.Visible = false;
                    ClientConnectingControl.Visible = false;
                    ClientConnectedControl.Visible = true;
                    return;
                default: return;
            }
        }

        private void OnDisconnectButtonPressed()
        {
            _client.Disconnect();
            _client.MessageReceived -= OnMessageReceivedDeferred;
            _client.StateChanged -= OnClientStateChangedDeferred;
            CallDeferred(nameof(ClearMessages));
        }

        private void ClearMessages()
        {
            MessagesOutput.Text = "";
        }

        private void OnSendMessageButtonPressed()
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
                return;

            _client.SendMessage(MessageInput.Text);
            MessageInput.Text = string.Empty;
        }

        private void PrintErrorMessageDeferred(string message)
        {
            CallDeferred(nameof(PrintErrorMessage), message);
        }

        private void PrintErrorMessage(string message)
        {
            ErrorsOutput.Text = message;
        }

        private void AddMessageToOutput(byte[] messageBytes)
        {
            MessagesOutput.Text += Encoding.UTF8.GetString(messageBytes);
        }

        public override void _ExitTree()
        {
            if (_client is not null)
                OnDisconnectButtonPressed();
            base._ExitTree();
        }
    }
}