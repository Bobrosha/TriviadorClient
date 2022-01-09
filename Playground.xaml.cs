using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TriviadorClient.Entities;

namespace TriviadorClient
{
    /// <summary>
    /// Логика взаимодействия для Playground.xaml
    /// </summary>
    public partial class Playground : Window
    {
        private readonly Client _Client;
        private readonly Player _ThisPlayer;

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
            List<TriviadorMap.Cell> listCells = _Client.GetMap().Cells;
            UIElementCollection localButtonMap = CanvasMap.Children;
            foreach (TriviadorMap.Cell cell in listCells)
            {
                //if (cell.OwnerId != null)
                //{
                //    SolidColorBrush brush = cell.OwnerId == 0 ? new SolidColorBrush(Color.FromRgb(255, 0, 0)) : new SolidColorBrush(Color.FromRgb(0, 255, 0));
                //    ((Button)localButtonMap[cell.Id - 1]).Background = brush;
                //}

                Button button1 = (Button)localButtonMap[0];
                Button button2 = (Button)localButtonMap[1];
            }
        }
    }
}
