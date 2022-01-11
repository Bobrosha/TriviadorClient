using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        private HashSet<Button> _NearestButtons;

        private double _DisplayTimer;
        private int _DeadTimer = 15;

        public Playground(Client client, Player thisPlayer)
        {
            _ThisPlayer = thisPlayer;
            _Client = client;

            //_ThisPlayer.Id = 0;

            InitializeComponent();
            LeaderBoard();
            Init();
        }

        private void Init()
        {
            LeaderBoard();
            _Client.GetWhoseTurn();
            _MineTurn = _ThisPlayer.Id == _Client.GetTurn();
            CreateMap(setActive: _MineTurn);

            DispatcherTimer timer = new(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromMilliseconds(300);
            timer.Tick += CheckTurn;

            if (!_MineTurn)
            {
                _DisplayTimer = 0;

                SolidColorBrush brush = _ThisPlayer.Id != 0 ? new SolidColorBrush(Color.FromRgb(255, 0, 0)) : new SolidColorBrush(Color.FromRgb(0, 255, 0));
                TextBlockTurn.Foreground = brush;
                TextBlockTurn.Visibility = Visibility.Visible;

                TextBlockTurnTimes.Foreground = brush;
                TextBlockTurnTimes.Visibility = Visibility.Visible;

                timer.Start();
            }
        }

        private void CheckTurn(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;

            _Client.GetWhoseTurn();
            _DisplayTimer += timer.Interval.TotalMilliseconds;
            TextBlockTurnTimes.Text = String.Format(CultureInfo.InvariantCulture, "{0:f1}", _DisplayTimer / 1000);
            if (_ThisPlayer.Id == _Client.GetTurn())
            {
                TextBlockTurn.Visibility = Visibility.Hidden;
                TextBlockTurnTimes.Visibility = Visibility.Hidden;
                timer.Stop();
                Init();
            }
        }

        private void LeaderBoard()
        {
            _Client.GetPlayersList();

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
            _NearestButtons = new HashSet<Button>();

            _Client.GetMapFromServer();
            List<Cell> listCells = _Client.GetMap().Cells;

            if (listCells[_ThisPlayer.Id != 0 ? 2 : 12].OwnerId == _ThisPlayer.Id)
            {
                foreach (var el in CanvasMap.Children.OfType<Button>().ToList())
                {
                    _NearestButtons.Add(el);
                }

                TextBlockTurn.Visibility = Visibility.Visible;
                TextBlockTurnTimes.Visibility = Visibility.Visible;
                TextBlockTurn.Text = "Вы Победили!";

                DispatcherTimer timer = new(DispatcherPriority.Normal);
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick_Event;
                timer.Start();
            }
            
            if (listCells[_ThisPlayer.Id == 0 ? 2 : 12].OwnerId != _ThisPlayer.Id)
            {
                foreach (var el in CanvasMap.Children.OfType<Button>().ToList())
                {
                    _NearestButtons.Add(el);
                }

                TextBlockTurn.Visibility = Visibility.Visible;
                TextBlockTurnTimes.Visibility = Visibility.Visible;
                TextBlockTurn.Text = "Вы Проиграли!";

                DispatcherTimer timer = new(DispatcherPriority.Normal);
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick_Event;
                timer.Start();
            }

            UIElementCollection localButtonMap = CanvasMap.Children;
            foreach (Cell cell in listCells)
            {
                if (cell.OwnerId != null)
                {
                    SolidColorBrush brush = cell.OwnerId == 0 ? new SolidColorBrush(Color.FromRgb(255, 0, 0)) : new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    Button button = (Button)localButtonMap[cell.Id - 1];
                    button.Background = brush;

                    if (cell.OwnerId == _ThisPlayer.Id && setActive)
                    {
                        foreach (int i in cell.NearestCells)
                        {
                            var nearestCell = listCells.Find(x => x.Id == i);
                            if (nearestCell.OwnerId != _ThisPlayer.Id)
                            {
                                Button nearestButton = (Button)localButtonMap[i - 1];
                                if (!_NearestButtons.Contains(nearestButton))
                                {
                                    _NearestButtons.Add(nearestButton);
                                    nearestButton.BorderBrush = brush;
                                    nearestButton.BorderThickness = new Thickness(5, 5, 5, 5);
                                    nearestButton.Click += Button_Click_StartQuestion;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Timer_Tick_Event(object sender, EventArgs e)
        {
            var enumer = _NearestButtons.GetEnumerator();
            enumer.MoveNext();
            var el = enumer.Current;
            el.Visibility = Visibility.Hidden;
            _NearestButtons.Remove(el);
            DispatcherTimer timer = sender as DispatcherTimer;
            _DeadTimer -= (int)timer.Interval.TotalSeconds;
            TextBlockTurnTimes.Text = _DeadTimer.ToString();
            if (_DeadTimer <= 0)
            {
                timer.Stop();
                this.Close();
            }
        }

        private void EndTurn(object sender, EventArgs e)
        {
            WindowPlayground.Visibility = Visibility.Visible;
            _Client.NextTurn();
            Init();
        }

        private void Button_Click_StartQuestion(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (WindowPlayground.Visibility != Visibility.Hidden)
            {
                foreach (Button btn in _NearestButtons)
                {
                    btn.Click -= Button_Click_StartQuestion;
                    btn.BorderThickness = new Thickness(1, 1, 1, 1);
                    btn.BorderBrush = new SolidColorBrush(Color.FromRgb(112, 112, 112));
                }

                _NearestButtons.Clear();

                var map = _Client.GetMap();

                int index = int.Parse((string)button.Tag);

                Cell currentCell = map.Cells[index];

                WindowPlayground.Visibility = Visibility.Hidden;
                var questionWindow = new Questions(_Client, currentCell, _ThisPlayer);
                questionWindow.Closed += new EventHandler(EndTurn);
                questionWindow.Show();
            }
        }
    }
}
