using System.Windows;
using WPF_project.Data.Models.Interfaces;

namespace WPF_project.Data.Views
{
    /// <summary>
    /// Interaction logic for Server.xaml
    /// </summary>
    public partial class ServerView : Window
    {
        public ServerView()
        {
            InitializeComponent();
        }

        private void OnServersSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            viewModel.Server = (IServer)serverInput.SelectedItem;
        }

        private void OnStartServerClick(object sender, RoutedEventArgs e)
        {
            viewModel.Start();
        }

        private void OnStopServerClick(object sender, RoutedEventArgs e)
        {
            viewModel.Stop();
        }
    }
}
