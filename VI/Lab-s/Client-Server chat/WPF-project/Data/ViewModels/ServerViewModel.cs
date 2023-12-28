using System.ComponentModel;
using System.Text;
using System.Windows;
using WPF_project.Data.Models.Implementations;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.ViewModels
{
    class ServerViewModel : ViewModelSharedBetweenClient
    {
        private readonly Server[] _servers = { new ServerUDP() };
        private Server _server = null!;
        public Server[] Servers => _servers;
        public Server Server
        {
            get => _server; set
            {
                _server = value;
                OnPropertyChanged(nameof(Server));
            }
        }
        public bool IsServerRunning => Server is not null && Server.IsRunning;
        public Visibility VisibleOnServerRunning => IsServerRunning ? Visibility.Visible : Visibility.Collapsed;
        public Visibility HiddenOnServerRunning => IsServerRunning ? Visibility.Collapsed : Visibility.Visible;
        public override bool AreInputParametersValid =>
            Server is not null && AreServerIPAddressAndPortValid;

        protected override void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Server))
            {
                OnPropertyChanged(nameof(AreInputParametersValid));
                OnPropertyChanged(nameof(IsServerRunning));
                Server.DataReceived += OnDataReceived;
                MessagesText = "";
            }
            else if (e.PropertyName == nameof(ServerIPAddressText)
                || e.PropertyName == nameof(ServerPortText))
            {
                OnPropertyChanged(nameof(AreInputParametersValid));
            }
            else if (e.PropertyName == nameof(AreInputParametersValid))
            {
                OnPropertyChanged(nameof(ErrorText));
            }
            else if (e.PropertyName == nameof(IsServerRunning))
            {
                MessagesText = "";
                OnPropertyChanged(nameof(VisibleOnServerRunning));
                OnPropertyChanged(nameof(HiddenOnServerRunning));
            }
        }

        public void Start()
        {
            Server.Start(_serverIPEndPoint);
            OnPropertyChanged(nameof(IsServerRunning));
        }

        public void Stop()
        {
            Server.Stop();
            OnPropertyChanged(nameof(IsServerRunning));
        }
    }
}