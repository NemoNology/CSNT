using System.ComponentModel;
using System.Net;
using System.Text;

namespace WPF_project.Data.ViewModels
{
    abstract class ViewModelSharedBetweenClient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected readonly IPEndPoint _ipEndPoint = new(IPAddress.Loopback, 40404);
        protected string _ipAddressText = string.Empty;
        protected string _portText = string.Empty;
        protected string _messagesText = string.Empty;
        public string IPAddressText
        {
            get => _ipAddressText;
            set
            {
                _ipAddressText = value;
                if (IPAddress.TryParse(_ipAddressText, out var address))
                    _ipEndPoint.Address = address;
                OnPropertyChanged(nameof(IPAddressText));
            }
        }
        public string PortText
        {
            get => _portText;
            set
            {
                _portText = value;
                if (ushort.TryParse(_portText, out var port))
                    _ipEndPoint.Port = port;
                OnPropertyChanged(nameof(PortText));
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
        public string ErrorText => IsInputParametersValid ? "" : "Входные параметры некорректны";
        public bool IsIPAddressAndPortValid => 
            IPAddress.TryParse(IPAddressText, out _)
            && ushort.TryParse(PortText, out _);
        abstract public bool IsInputParametersValid { get; }

        public ViewModelSharedBetweenClient()
        {
            _ipAddressText = _ipEndPoint.Address.ToString();
            _portText = _ipEndPoint.Port.ToString();
            PropertyChanged += OnPropertyChanged;
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected abstract void OnPropertyChanged(object? sender, PropertyChangedEventArgs e);

        protected virtual void OnDataReceived(object? sender, byte[] data)
        {
            MessagesText += $"{Encoding.UTF8.GetString(data)}\n";
        }
    }
}
