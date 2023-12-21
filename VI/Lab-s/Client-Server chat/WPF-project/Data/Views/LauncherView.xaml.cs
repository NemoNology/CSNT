using System.ComponentModel;
using System.Windows;

namespace WPF_project.Data.Views
{
    /// <summary>
    /// Interaction logic for Launcher.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        public bool IsLauncherClosingAfterProgramLaunching { get; set; }

        private void OnLaunchClientClicked(object sender, RoutedEventArgs e)
        {
            new Client().Show();
            CloseIfNeeded();
        }

        private void OnLaunchServerClicked(object sender, RoutedEventArgs e)
        {
            new Server().Show();
            CloseIfNeeded();
        }

        private void CloseIfNeeded()
        {
            if (IsLauncherClosingAfterProgramLaunching)
            {
                Close();
            }
        }
    }
}
