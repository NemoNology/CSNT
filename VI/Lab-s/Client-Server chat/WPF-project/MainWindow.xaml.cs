using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Media;
using WPF_project.Data.Models.Implementations;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        PropertyChangeNotify(nameof(IsInputStartClientParametersCorrect));
    }

    public IClient[] Clients { get; set; } = { new ClientUDP() };
    public IServer[] Servers { get; private set; }
    public IClient Client => (inputClient is null
        || inputClient.SelectedIndex < 0) ? null! : Clients[inputClient.SelectedIndex];
    //public IServer Server => (inputServer is null 
    //|| inputServer.SelectedIndex < 0) ? null! : Servers[inputServer.SelectedIndex];
    public bool IsInputStartClientParametersCorrect
    {
        get
        {
            if (outputClientInputsError is null)
                return false;

            if (Client is null)
            {
                outputClientInputsError.Content = "Client not chosen";
                return false;
            }

            var isPortCorrect = ushort.TryParse(inputClientPort.Text, out ushort port);
            if (!isPortCorrect)
            {
                outputClientInputsError.Content = "Port is incorrect";
                return false;
            }

            foreach (var b in inputClientIP.Text.Split('.'))
            {
                if (!byte.TryParse(b, out _))
                {
                    outputClientInputsError.Content = "IP-address is incorrect";
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(inputClientUsername.Text))
            {
                outputClientInputsError.Content = "Username can not be empty";
                return false;
            }

            outputClientInputsError.Content = "";
            Client.IPEndPoint = new(IPAddress.Parse(inputClientIP.Text), port);
            return true;
        }
    }
    public bool IsClientConnected
    {
        get
        {
            if (Client is null)
                return false;

            return Client.IsConnected;
        }
    }
    public bool IsClientNotConnected => !IsClientConnected;
    public event PropertyChangedEventHandler? PropertyChanged;
    string FullMessage => inputClientUsername.Text + inputClientMessage.Text;
    public string ConnectDisconnectButtonContent =>
        IsClientConnected ? "Disconnect" : "Connect";

    private void PropertyChangeNotify(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnClientInputParametersChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        PropertyChangeNotify(nameof(IsInputStartClientParametersCorrect));
    }

    private void OnClientSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        PropertyChangeNotify(nameof(Client));
        PropertyChangeNotify(nameof(IsInputStartClientParametersCorrect));
    }

    private void OnConnectDisconnectClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (IsClientConnected)
            {
                Client.Disconnect();
            }
            else
            {
                Client.Connect();
            }
            outputClientInputsError.Content = string.Empty;
        }
        catch (Exception exc)
        {
            outputClientInputsError.Content = exc.Message;
        }

        PropertyChangeNotify(nameof(IsClientConnected));
        PropertyChangeNotify(nameof(IsClientNotConnected));
        PropertyChangeNotify(nameof(ConnectDisconnectButtonContent));
    }

    private void OnServerMessageReceived(object? sender, byte[] bytes)
    {
        outputClientMessages.Text += $"\n{Encoding.UTF8.GetString(bytes)}";
    }

    private void OnInputClientMessageClick(object sender, RoutedEventArgs e)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(FullMessage);
        Client.SendBytes(bytes);
        inputClientMessage.Text = string.Empty;
    }
}