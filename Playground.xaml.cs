using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
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

        private bool _MineTurn;
        private DispatcherTimer _Timer;

        public Playground(Client client, Player thisPlayer)
        {
            _ThisPlayer = thisPlayer;
            _Client = client;

            _ThisPlayer.Id = 0;

            InitializeComponent();
            LeaderBoard();
            Init();
        }

        private void Init()
        {
            _Client.GetWhoseTurn();
            _MineTurn = _ThisPlayer.Id == _Client.GetTurn();
            CreateMap(setActive: _MineTurn);

            _Timer = new(DispatcherPriority.Normal);
            _Timer.Interval = TimeSpan.FromMilliseconds(300);
            _Timer.Tick += CheckTurn;

            if (!_MineTurn)
            {
                _Timer.Start();
            }
        }

        private void CheckTurn(object sender, EventArgs e)
        {
            _Client.GetWhoseTurn();
            if (_Client.GetTurn() == _ThisPlayer.Id)
            {
                _Timer.Stop();
            }
        }

        //public void Init()
        //{
        //    _Client.GetWhoseTurn();
        //    while (_Client.GetTurn() != _ThisPlayer.Id)
        //    {
        //        Thread.Sleep(500);
        //        _Client.GetWhoseTurn();
        //    }
        //}

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

        private void CreateMap(bool setActive)
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
                                nearestButton.BorderBrush = button.BorderBrush;
                                if (setActive)
                                {
                                    nearestButton.BorderBrush = brush;
                                    nearestButton.BorderThickness = new Thickness(5, 5, 5, 5);
                                    nearestButton.Click += Button_Click_StartQuestion;
                                }
                                else
                                {
                                    nearestButton.BorderBrush = button.BorderBrush;
                                    nearestButton.BorderThickness = new Thickness(1, 1, 1, 1);
                                    nearestButton.Click -= Button_Click_StartQuestion;
                                }
                            }
                        }
                    }
                }
            }
        }

        //private void UpdateMapAndSetActive()
        //{
        //    _Client.GetMapFromServer();
        //    List<Cell> listCells = _Client.GetMap().Cells;
        //    UIElementCollection localButtonMap = CanvasMap.Children;
        //    foreach (Cell cell in listCells)
        //    {
        //        if (cell.OwnerId != null)
        //        {
        //            SolidColorBrush brush = cell.OwnerId == 0 ? new SolidColorBrush(Color.FromRgb(255, 0, 0)) : new SolidColorBrush(Color.FromRgb(0, 255, 0));
        //            Button button = (Button)localButtonMap[cell.Id - 1];
        //            button.Background = brush;

        //            if (cell.OwnerId == _ThisPlayer.Id)
        //            {
        //                foreach (int i in cell.NearestCells)
        //                {
        //                    var nearestCell = listCells.Find(x => x.Id == i);
        //                    if (nearestCell.OwnerId != _ThisPlayer.Id)
        //                    {
        //                        Button nearestButton = (Button)localButtonMap[i - 1];
        //                        nearestButton.BorderBrush = brush;
        //                        nearestButton.BorderThickness = new Thickness(5, 5, 5, 5);
        //                        nearestButton.Click += Button_Click_StartQuestion;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void VisibleOn(object sender, EventArgs e)
        {
            WindowPlayground.Visibility = Visibility.Visible;
        }


        private void Button_Click_StartQuestion(object sender, RoutedEventArgs e)
        {
            WindowPlayground.Visibility = Visibility.Hidden;
            var a = new Questions(_Client);
            a.Closed += new EventHandler(VisibleOn);
            a.Show();
        }
    }
}
