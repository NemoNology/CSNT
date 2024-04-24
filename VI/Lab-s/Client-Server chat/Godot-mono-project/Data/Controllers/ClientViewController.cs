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
        public VBoxContainer MessagesContainer { get; set; }
        [Export]
        public LineEdit MessageInput { get; set; }

        private void OnConnectButtonPressed()
        {
            ushort clientPort, serverPort;
            IPAddress serverIpAddress;
            bool isUdp = ProtocolInput.Selected == 0;
            if (!IPAddress.TryParse(ClientIpInput.Text, out IPAddress clientIpAddress))
            {
                CallDeferred(nameof(PrintErrorMessage), "Неверный IP-адрес клиента");
                return;
            }
            else if (!ushort.TryParse(ClientPortInput.Text, out clientPort))
            {
                CallDeferred(nameof(PrintErrorMessage), "Неверный порт клиента");
                return;
            }
            else if (!IPAddress.TryParse(ServerIpInput.Text, out serverIpAddress))
            {
                CallDeferred(nameof(PrintErrorMessage), "Неверный IP-адрес сервера");
                return;
            }
            else if (!ushort.TryParse(ServerPortInput.Text, out serverPort))
            {
                CallDeferred(nameof(PrintErrorMessage), "Неверный порт сервера");
                return;
            }
            else if (!NetHelper.IsAddressForTransportProtocolAvailable(new IPEndPoint(clientIpAddress, clientPort), isUdp))
            {
                CallDeferred(nameof(PrintErrorMessage), "Данный адрес для клиента занят");
                return;
            }
            else if (!NetHelper.IsThereActiveListenerWithSpecifiedAddress(new IPEndPoint(serverIpAddress, serverPort), isUdp))
            {
                CallDeferred(nameof(PrintErrorMessage), "Не найден слушатель (сервер) с введёнными данными");
                return;
            }

            CallDeferred(nameof(PrintErrorMessage), "");
            _client = isUdp ? new ClientUdp() : new ClientTcp();
            _client.MessageReceived += OnMessageRecieved;

            try
            {
                _client.Connect(clientIpAddress, clientPort, serverIpAddress, serverPort);
                CallDeferred(nameof(SwitchControlsVisibility));
            }
            catch (Exception e)
            {
                CallDeferred(nameof(PrintErrorMessage), "Не удалось подключиться:\n" + e.Message);
            }
        }

        private void OnDisconnectButtonPressed()
        {
            _client.Disconnect();
            _client.MessageReceived -= OnMessageRecieved;
            CallDeferred(nameof(ClearMessages));
            CallDeferred(nameof(SwitchControlsVisibility));
        }

        private void SwitchControlsVisibility()
        {
            ClientConnectedControl.Visible = !ClientConnectedControl.Visible;
            ClientDisconnectedControl.Visible = !ClientDisconnectedControl.Visible;
        }

        private void ClearMessages()
        {
            foreach (Node child in MessagesContainer.GetChildren())
                MessagesContainer.RemoveChild(child);
        }

        private void OnSendMessageButtonPressed()
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
                return;

            _client.SendMessage(MessageInput.Text);
            MessageInput.Text = string.Empty;
        }

        private void OnMessageRecieved(byte[] messageBytes)
        {
            if (messageBytes.Length == 0
                || Enumerable.SequenceEqual(messageBytes, ServerTcp.CloseMessageBytes))
            {
                OnDisconnectButtonPressed();
                CallDeferred(nameof(PrintErrorMessage), "Сервер был остановлен");
                return;
            }
            CallDeferred(nameof(AddMessageAsChild), messageBytes);
        }

        private void PrintErrorMessage(string message)
        {
            ErrorsOutput.Text = message;
        }

        private void AddMessageAsChild(byte[] messageBytes)
        {
            MessagesContainer.AddChild(new Label { Text = Encoding.UTF8.GetString(messageBytes) });
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            _client?.Disconnect();
        }
    }
}