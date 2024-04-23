using CSNT.Clientserverchat.Data.Models;
using Godot;
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
                ErrorsOutput.Text = "Неверный IP-адрес клиента";
                return;
            }
            else if (!ushort.TryParse(ClientPortInput.Text, out clientPort))
            {
                ErrorsOutput.Text = "Неверный порт клиента";
                return;
            }
            else if (!IPAddress.TryParse(ServerIpInput.Text, out serverIpAddress))
            {
                ErrorsOutput.Text = "Неверный IP-адрес сервера";
                return;
            }
            else if (!ushort.TryParse(ServerPortInput.Text, out serverPort))
            {
                ErrorsOutput.Text = "Неверный порт сервера";
                return;
            }
            else if (!NetHelper.IsAddressForTransportProtocolAvailable(new IPEndPoint(clientIpAddress, clientPort), isUdp))
            {
                ErrorsOutput.Text = "Данный порт для клиента занят";
                return;
            }
            else if (!NetHelper.IsThereActiveListenerWithSpecifiedAddress(new IPEndPoint(serverIpAddress, serverPort), isUdp))
            {
                ErrorsOutput.Text = "Не найден сервер с введёнными данными";
                return;
            }

            ErrorsOutput.Text = string.Empty;
            _client = isUdp ? new ClientUdp() : new ClientTcp();
            _client.MessageReceived += OnMessageRecieved;

            _client.Connect(clientIpAddress, clientPort, serverIpAddress, serverPort);
            ClientConnectedControl.Visible = true;
            ClientDisconnectedControl.Visible = false;
        }

        private void OnDisconnectButtonPressed()
        {
            _client.Dissconnect();
            _client.MessageReceived -= OnMessageRecieved;
            CallDeferred(nameof(ClearMessages));
            ClientDisconnectedControl.Visible = true;
            ClientConnectedControl.Visible = false;
        }

        private void ClearMessages()
        {
            foreach (Node child in MessagesContainer.GetChildren())
                MessagesContainer.RemoveChild(child);
        }

        private void OnSendMessageButtonPressed()
        {
            _client.SendMessage(MessageInput.Text);
        }

        private void OnMessageRecieved(byte[] messageBytes)
        {
            CallDeferred(nameof(AddMessageAsChild), messageBytes);
        }

        private void AddMessageAsChild(byte[] messageBytes)
        {
            MessagesContainer.AddChild(new Label { Text = Encoding.UTF8.GetString(messageBytes) });
        }

        ~ClientViewController()
        {
            _client?.Dissconnect();
        }
    }
}