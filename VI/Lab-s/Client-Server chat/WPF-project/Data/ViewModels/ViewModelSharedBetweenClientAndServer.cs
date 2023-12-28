using System.ComponentModel;
using System.Net;
using System.Text;

namespace WPF_project.Data.ViewModels
{
    abstract class ViewModelSharedBetweenClient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected readonly IPEndPoint _serverIPEndPoint = new(IPAddress.Loopback, 40404);
        protected string _serverIPAddressText = string.Empty;
        protected string _serverPortText = string.Empty;
        protected string _messagesText = string.Empty;
        public string ServerIPAddressText
        {
            get => _serverIPAddressText;
            set
            {
                _serverIPAddressText = value;
                OnPropertyChanged(nameof(ServerIPAddressText));
            }
        }
        public string ServerPortText
        {
            get => _serverPortText;
            set
            {
                _serverPortText = value;
                OnPropertyChanged(nameof(ServerPortText));
            }
        }
        public string MessagesText
        {
            get => _messagesText;
            set
            {
                _messagesText = value;
                OnPropertyChanged(nameof(MessagesText));
            }
        }
        public string ErrorText => AreInputParametersValid ? "" : "Входные параметры некорректны";
        public bool AreServerIPAddressAndPortValid
        {
            get
            {
                var isIPAddressValid = IPAddress.TryParse(ServerIPAddressText, out var ipAddress);
                var isPortValid = ushort.TryParse(ServerPortText, out var port);

                if (isIPAddressValid)
                    _serverIPEndPoint.Address = ipAddress!;
                if (isPortValid)
                    _serverIPEndPoint.Port = port!;

                return isIPAddressValid && isPortValid;
            }
        }
        public abstract bool AreInputParametersValid { get; }

        public ViewModelSharedBetweenClient()
        {
            _serverIPAddressText = _serverIPEndPoint.Address.ToString();
            _serverPortText = _serverIPEndPoint.Port.ToString();
            PropertyChanged += OnPropertyChanged;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected abstract void OnPropertyChanged(object? sender, PropertyChangedEventArgs e);

        protected void OnDataReceived(object? sender, byte[] data)
        {
            MessagesText += $"{Encoding.UTF8.GetString(data)} {{{DateTime.Now}}}\n";
        }
    }
}
