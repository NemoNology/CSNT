using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows;
using WPF_project.Data.Models.Implementations;
using WPF_project.Data.Models.Interfaces;
using WPF_project.Data.Views;

namespace WPF_project.Data.ViewModels
{
    class ClientViewModel : ViewModelSharedBetweenClient
    {
        private readonly Client[] _clients = { new ClientUDP() };
        private Client _client = null!;
        private string _messageText = string.Empty;
        private string _username = "Некто";
        private bool? _connectionStatement = true;
        public Client[] Clients => _clients;
        public Client Client
        {
            get => _client; set
            {
                _client = value;
                OnPropertyChanged(nameof(Client));
            }
        }
        public bool? ConnectionStatement
        {
            get => _connectionStatement;
            private set
            {
                _connectionStatement = value;
                OnPropertyChanged(nameof(ConnectionStatement));
            }
        }
        public bool IsClientConnected =>
            Client is not null && Client.IsConnected;
        public bool IsClientConnecting =>
            _connectionStatement is null || !(bool)_connectionStatement;
        public Visibility VisibleOnClientConnecting =>
            IsClientConnecting ? Visibility.Visible : Visibility.Collapsed;
        public Visibility VisibleOnClientConnected =>
            IsClientConnected ? Visibility.Visible : Visibility.Collapsed;
        public Visibility HiddenOnClientConnectedOrConnecting =>
            IsClientConnected || IsClientConnecting ? Visibility.Collapsed : Visibility.Visible;
        public bool EnabledOnMessageValid =>
            !string.IsNullOrWhiteSpace(MessageText);
        public override bool IsInputParametersValid =>
            Client is not null
            && IsIPAddressAndPortValid
            && !string.IsNullOrWhiteSpace(Username);
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }
        public string ConnectionStatementText =>
            _connectionStatement is null ? "Подключение..." : (
            (bool)_connectionStatement ?
                "Успешно подключено" : "Не получилось подключиться;\nПерепроверьте входные данные и попытайтесь снова;"
            );

        public void ResetConnectionStatement()
        {
            ConnectionStatement = true;
        }

        protected override void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Client))
            {
                OnPropertyChanged(nameof(IsInputParametersValid));
                OnPropertyChanged(nameof(IsClientConnected));
                Client.DataReceived += OnDataReceived;
                MessagesText = "";
            }
            else if (e.PropertyName == nameof(IPAddressText)
                || e.PropertyName == nameof(PortText))
            {
                OnPropertyChanged(nameof(IsInputParametersValid));
            }
            else if (e.PropertyName == nameof(IsInputParametersValid))
            {
                OnPropertyChanged(nameof(ErrorText));
            }
            else if (e.PropertyName == nameof(IsClientConnected))
            {
                MessagesText = "";
                OnPropertyChanged(nameof(VisibleOnClientConnected));
                OnPropertyChanged(nameof(HiddenOnClientConnectedOrConnecting));
            }
            else if (e.PropertyName == nameof(MessageText))
            {
                OnPropertyChanged(nameof(EnabledOnMessageValid));
            }
            else if (e.PropertyName == nameof(ConnectionStatement))
            {
                OnPropertyChanged(nameof(IsClientConnecting));
                OnPropertyChanged(nameof(IsClientConnected));
                OnPropertyChanged(nameof(ConnectionStatementText));
            }
            else if (e.PropertyName == nameof(IsClientConnecting))
            {
                OnPropertyChanged(nameof(VisibleOnClientConnecting));
                OnPropertyChanged(nameof(HiddenOnClientConnectedOrConnecting));
            }
        }

        public async void Connect()
        {
            ConnectionStatement = null;
            ConnectionStatement = await Client.Connect(_ipEndPoint, new TimeSpan(0, 0, 3));   
        }

        public void Disconnect()
        {
            Client.Disconnect();
            ResetConnectionStatement();
            OnPropertyChanged(nameof(IsClientConnected));
        }

        public void SendMessage()
        {
            Client.SendData(Encoding.UTF8.GetBytes($"{Username}: {MessageText}"));
            MessageText = string.Empty;
        }
    }
}
