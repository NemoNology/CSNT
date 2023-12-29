using System.Windows;

namespace WPF_project.Data.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            viewModel.Start();
        }

        private void OnStopClick(object sender, RoutedEventArgs e)
        {
            viewModel.Stop();
        }
    }
}