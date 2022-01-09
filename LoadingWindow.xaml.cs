using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TriviadorClient.Entities;

namespace TriviadorClient
{
    /// <summary>
    /// Логика взаимодействия для LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private readonly Client _Client;
        private readonly Player _ThisPlayer;

        public LoadingWindow(Client client, string nickName)
        {
            _Client = client;
            InitializeComponent();
            ListPlayers();
            _ThisPlayer = _Client.GetMap().Players.Find(player => player.Name.Equals(nickName));
        }

        private void ListPlayers()
        {
            _Client.GetPlayersList();
            List<Player> listPlayers = _Client.GetMap().Players;
            ListBoxPlayers.ItemsSource = from player in listPlayers select player.Name;
        }

        private void Button_Click_StartGame(object sender, RoutedEventArgs e)
        {
            if (_Client.GetReadyStatus())
            {
                WindowLoading.Visibility = Visibility.Hidden;
                new Playground(_Client, _ThisPlayer).Show();
                WindowLoading.Close();
            }
            else
            {
                TextBlockGameSessionNotReady.Visibility = Visibility.Visible;
            }
        }
    }
}
