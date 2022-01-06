using System.Windows;
using TriviadorClient.Entities;

namespace TriviadorClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Client _Client;

        public MainWindow()
        {
            _Client = Startup.GetClient();
            InitializeComponent();
        }

        private void Button_Click_AddPlayer(object sender, RoutedEventArgs e)
        {
            string nickName = TextBoxNickName.Text;
            if (string.IsNullOrWhiteSpace(nickName))
            {
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }
            _Client.AddPlayer(TextBoxNickName.Text);
            WindowAuthorization.Visibility = Visibility.Hidden;
            new Lobby();
        }
    }
}
