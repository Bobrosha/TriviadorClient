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
   /// Логика взаимодействия для Playground.xaml
   /// </summary>
   public partial class Playground : Window
   {
      private Client _Client;
      private Player _ThisPlayer;

      public Playground(Client client, Player thisPlayer)
      {
         _ThisPlayer = thisPlayer;
         _Client = client;
         InitializeComponent();
         LeaderBoard();
         CreateMap();
      }
      
      private void LeaderBoard()
      {
         var player1 = _Client.GetMap().Players[0];
         var player2 = _Client.GetMap().Players[1];

         NickName1.Text = player1.Name;
         NickName2.Text = player2.Name;

         Score1.Text = player1.Score.ToString();
         Score2.Text = player2.Score.ToString();

         PlayerPoint1.Fill = Brushes.Red;
         PlayerPoint2.Fill = Brushes.Green;
      }

      private void CreateMap()
      {
         var listCells = _Client.GetMap().Cells;
         var localButtonMap = CanvasMap.Children; 
         foreach(var cell in listCells) 
         {
            ((Button)localButtonMap[cell.Id - 1]).Content += $" cell ID = {cell.Id}";
         }
      }
   }
}
