using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
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

        private DispatcherTimer _Timer;

        public LoadingWindow(Client client, string nickName)
        {
            _Client = client;
            _ThisPlayer = _Client.GetMap().Players.Find(player => player.Name.Equals(nickName, StringComparison.Ordinal));

            InitializeComponent();

            _Timer = new(DispatcherPriority.Normal);
            _Timer.Interval = TimeSpan.FromSeconds(1);
            _Timer.Tick += Tick_Event_UpdateListPlayers;
            _Timer.Start();
        }

        private void Tick_Event_UpdateListPlayers(object sender, EventArgs e)
        {
            ListPlayers();
        }

        private void ListPlayers()
        {
            _Client.GetPlayersList();
            List<Player> listPlayers = _Client.GetMap().Players;
            ListBoxPlayers.ItemsSource = from player in listPlayers select player.Name;
        }

        private void Button_Click_Event_StartGame(object sender, RoutedEventArgs e)
        {
            if (_Client.GetReadyStatus())
            {
                new Playground(_Client, _ThisPlayer).Show();
                _Timer.Stop();
                WindowLoading.Close();
            }
            else
            {
                TextBlockGameSessionNotReady.Visibility = Visibility.Visible;
            }
        }
    }
}
