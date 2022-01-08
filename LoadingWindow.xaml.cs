using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TriviadorClient.Entities;

namespace TriviadorClient
{
   /// <summary>
   /// Логика взаимодействия для LoadingWindow.xaml
   /// </summary>
   public partial class LoadingWindow : Window
   {
      private Client _Client;
      private Player _ThisPlayer;

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
         WindowLoading.Visibility = Visibility.Hidden;
         new Playground(_Client, _ThisPlayer).Show();
         WindowLoading.Close();
      }
   }
}
