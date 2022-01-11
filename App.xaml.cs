using System.Windows;

namespace TriviadorClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Exit(object sender, ExitEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}