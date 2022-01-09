using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TriviadorClient.Entities;
using static TriviadorClient.Entities.TriviadorMap;

namespace TriviadorClient
{
    /// <summary>
    /// Логика взаимодействия для Playground.xaml
    /// </summary>
    public partial class Playground : Window
    {
        private readonly Client _Client;
        private readonly Player _ThisPlayer;
        private bool _OurMove;

        public Playground(Client client, Player thisPlayer)
        {
            _ThisPlayer = thisPlayer;
            _Client = client;

            _ThisPlayer.Id = 0;

            InitializeComponent();
            LeaderBoard();
            CreateMap();
            _Client.GetWhoseTurn();
            while (_Client.GetTurn() != _ThisPlayer.Id)
            {
                // asdawdadsawd PEREDELAT
                _ = Task.Delay(1000).ContinueWith(_ =>
                  {
                      _Client.GetWhoseTurn();
                  });
            }
            UpdateMapAndSetActive();
        }

        public void Init()
        {
            _Client.GetWhoseTurn();
            while (_Client.GetTurn() != _ThisPlayer.Id)
            {
                Thread.Sleep(500);
                _Client.GetWhoseTurn();
            }
            UpdateMapAndSetActive();
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
            List<Cell> listCells = _Client.GetMap().Cells;
            UIElementCollection localButtonMap = CanvasMap.Children;
            foreach (Cell cell in listCells)
            {
                if (cell.OwnerId != null)
                {
                    SolidColorBrush brush = cell.OwnerId == 0 ? new SolidColorBrush(Color.FromRgb(255, 0, 0)) : new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    Button button = (Button)localButtonMap[cell.Id - 1];
                    button.Background = brush;

                    if (cell.OwnerId == _ThisPlayer.Id)
                    {
                        foreach (int i in cell.NearestCells)
                        {
                            var nearestCell = listCells.Find(x => x.Id == i);
                            if (nearestCell.OwnerId != _ThisPlayer.Id)
                            {
                                Button nearestButton = (Button)localButtonMap[i - 1];
                                nearestButton.BorderBrush = button.BorderBrush;
                                nearestButton.BorderThickness = new Thickness(1, 1, 1, 1);
                                nearestButton.Click -= NearestButton_Click_StartBattle;
                            }
                        }
                    }
                }
            }
        }

        private void UpdateMapAndSetActive()
        {
            _Client.GetMapFromServer();
            List<Cell> listCells = _Client.GetMap().Cells;
            UIElementCollection localButtonMap = CanvasMap.Children;
            foreach (Cell cell in listCells)
            {
                if (cell.OwnerId != null)
                {
                    SolidColorBrush brush = cell.OwnerId == 0 ? new SolidColorBrush(Color.FromRgb(255, 0, 0)) : new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    Button button = (Button)localButtonMap[cell.Id - 1];
                    button.Background = brush;

                    if (cell.OwnerId == _ThisPlayer.Id)
                    {
                        foreach (int i in cell.NearestCells)
                        {
                            var nearestCell = listCells.Find(x => x.Id == i);
                            if (nearestCell.OwnerId != _ThisPlayer.Id)
                            {
                                Button nearestButton = (Button)localButtonMap[i - 1];
                                nearestButton.BorderBrush = brush;
                                nearestButton.BorderThickness = new Thickness(5, 5, 5, 5);
                                nearestButton.Click += NearestButton_Click_StartBattle;
                            }
                        }
                    }
                }
            }
        }

        private void NearestButton_Click_StartBattle(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int cellId = int.Parse((string)button.Tag);

            Cell cell = _Client.GetMap().Cells.Find(x => x.Id == cellId);

            WindowPlayground.Visibility = Visibility.Hidden;
            new BattleForCell(cell, _Client).Show();
            WindowPlayground.Visibility = Visibility.Visible;
            Init();
        }
    }
}
