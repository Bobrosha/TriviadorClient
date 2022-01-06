using System.Linq;
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
                TextBlockWrongNickName.Text = "Не подходящее имя!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            var names = from player in _Client.GetMap().Players select player.Name;

            if (names.Contains(nickName))
            {
                TextBlockWrongNickName.Text = "Такой ник уже существует!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            _Client.AddPlayer(TextBoxNickName.Text);
            WindowAuthorization.Visibility = Visibility.Hidden;
            new LoadingWindow(_Client).Show();
            WindowAuthorization.Close();
        }
    }
}