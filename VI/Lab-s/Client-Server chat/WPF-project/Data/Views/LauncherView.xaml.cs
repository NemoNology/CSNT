using System.ComponentModel;
using System.Windows;

namespace WPF_project.Data.Views
{
    /// <summary>
    /// Interaction logic for Launcher.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        private bool _isLaucnherCloseAfterProgramLaunch = false;
        public bool IsLauncherCloseAfterProgramLaunch
        {
            get => _isLaucnherCloseAfterProgramLaunch;
            set { _isLaucnherCloseAfterProgramLaunch = value; }
        }

        public Launcher()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CloseIfNeeded()
        {
            if (_isLaucnherCloseAfterProgramLaunch)
            {
                Close();
            }
        }

        private void OnLaunchClientClick(object sender, RoutedEventArgs e)
        {
            new Client().Show();
            CloseIfNeeded();
        }

        private void OnLaunchServerClick(object sender, RoutedEventArgs e)
        {
            new ServerView().Show();
            CloseIfNeeded();
        }
    }
}
