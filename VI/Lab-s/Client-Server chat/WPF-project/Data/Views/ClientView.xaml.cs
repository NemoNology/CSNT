using System.Windows;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Views
{
    /// <summary>
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class ClientView : Window
    {
        public ClientView()
        {
            InitializeComponent();
        }

        private void OnClientsSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            viewModel.Client = (Client)clientInput.SelectedItem;
        }

        private void OnConnectClick(object sender, RoutedEventArgs e)
        {
            viewModel.Connect();
        }

        private void OnDisconnectClick(object sender, RoutedEventArgs e)
        {
            viewModel.Disconnect();
        }

        private void OnSendMessageClick(object sender, RoutedEventArgs e)
        {
            viewModel.SendMessage();
        }

        private void OnReturnClick(object sender, RoutedEventArgs e)
        {
            viewModel.ResetConnectionStatement();
        }
    }
}
