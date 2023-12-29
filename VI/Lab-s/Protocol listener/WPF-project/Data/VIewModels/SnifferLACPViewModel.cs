using System.ComponentModel;
using System.Linq;
using System.Windows;
using WPF_project.Data.Models.Implementations;

namespace WPF_project.Data.ViewModels
{
    class SnifferLACPViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly SnifferLACP _sniffer = new();
        private string _snifferItemsText = "";
        public bool IsRunning => _sniffer.IsRunning;
        public Visibility VisibleOnSnifferRunning
            => IsRunning ? Visibility.Visible : Visibility.Collapsed;
        public Visibility HiddenOnSnifferRunning
            => IsRunning ? Visibility.Collapsed : Visibility.Visible;
        public string SnifferItemsText
        {
            get => _snifferItemsText;
            set
            {
                _snifferItemsText = value;
                OnPropertyChanged(nameof(SnifferItemsText));
            }
        }

        public SnifferLACPViewModel()
        {
            PropertyChanged += OnPropertyChanged;
            _sniffer.PacketReceived += OnPacketReceived;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }

        public void Start()
        {
            SnifferItemsText = "";
            _sniffer.Start();
            OnPropertyChanged(nameof(IsRunning));
        }

        public void Stop()
        {
            _sniffer.Stop();
            OnPropertyChanged(nameof(IsRunning));
        }

        public void OnPacketReceived(object? sender, byte[] data)
        {
            byte[] macD = data[..6];
            byte[] macS = data[6..12];
            string strD = "Destination: ";
            string strS = "Source: ";
            for (int i = 0; i < 6; i += 1)
            {
                strS += macS[i].ToString("X") + ":";
                strD += macD[i].ToString("X") + ":";
            }


            SnifferItemsText += strS + strD + "\n";
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsRunning))
            {
                OnPropertyChanged(nameof(VisibleOnSnifferRunning));
                OnPropertyChanged(nameof(HiddenOnSnifferRunning));
            }
        }
    }
}