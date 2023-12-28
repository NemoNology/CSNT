using System.ComponentModel;
using System.Text;
using System.Net;
using System.Windows;
using WPF_project.Data.Models.Implementations;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.ViewModels
{
    class ClientViewModel : ViewModelSharedBetweenClient
    {
        private readonly Client[] _clients = { new ClientUDP() };
        private CancellationTokenSource _awaitingConnectionCancalletionSource = null!;
        private readonly IPEndPoint _clientIPEndPoint = new(IPAddress.Loopback, 40405);
        private string _clientIPAddressText = string.Empty;
        private string _clientPortText = string.Empty;
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
        public string ClientIPAddressText
        {
            get => _clientIPAddressText;
            set
            {
                _clientIPAddressText = value;
                OnPropertyChanged(nameof(ClientIPAddressText));
            }
        }
        public string ClientPortText
        {
            get => _clientPortText;
            set
            {
                _clientPortText = value;
                OnPropertyChanged(nameof(ClientPortText));
            }
        }
        public bool IsClientIPAddressAndPortValid
        {
            get
            {
                var isIPAddressValid = IPAddress.TryParse(ClientIPAddressText, out var ipAddress);
                var isPortValid = ushort.TryParse(ClientPortText, out var port);

                if (isIPAddressValid)
                    _clientIPEndPoint.Address = ipAddress!;
                if (isPortValid)
                    _clientIPEndPoint.Port = port!;

                return isIPAddressValid && isPortValid;
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
        public override bool AreInputParametersValid =>
            Client is not null
            && AreServerIPAddressAndPortValid
            && IsClientIPAddressAndPortValid
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
                "Успешно подключено" : "Не получилось подключиться/Потеряно соединение с сервером"
            );

        public ClientViewModel() : base()
        {
            _clientIPAddressText = _clientIPEndPoint.Address.ToString();
            _clientPortText = _clientIPEndPoint.Port.ToString();
        }

        public void ResetConnectionStatement()
        {
            _awaitingConnectionCancalletionSource.Cancel();
            ConnectionStatement = true;
            Disconnect();
        }

        protected override void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Client))
            {
                OnPropertyChanged(nameof(AreInputParametersValid));
                OnPropertyChanged(nameof(IsClientConnected));
                Client.DataReceived += OnDataReceived;
                Client.ConnectionWithServerLost += OnServerConnectionLost;
                MessagesText = "";
            }
            else if (e.PropertyName == nameof(ServerIPAddressText)
                || e.PropertyName == nameof(ServerPortText)
                || e.PropertyName == nameof(ClientIPAddressText)
                || e.PropertyName == nameof(ClientPortText))
            {
                OnPropertyChanged(nameof(AreInputParametersValid));
            }
            else if (e.PropertyName == nameof(AreInputParametersValid))
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

        private void OnServerConnectionLost(object? sender, EventArgs e)
        {
            ConnectionStatement = false;
        }
        
        public async void Connect()
        {
            ConnectionStatement = null;
            _awaitingConnectionCancalletionSource = new();
            ConnectionStatement = await Client.Connect(
                _clientIPEndPoint,
                _serverIPEndPoint,
                _awaitingConnectionCancalletionSource.Token);
        }

        public void Disconnect()
        {
            Client.Disconnect();
            if (IsClientConnecting)
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
