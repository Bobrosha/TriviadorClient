using System;
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
    /// Логика взаимодействия для Questions.xaml
    /// </summary>
    public partial class Questions : Window
    {
        private readonly Player _ThisPlayer;
        private readonly Client _Client;
        private DispatcherTimer _Timer;
        private Question _Question;
        private Cell _Cell;

        private int _DeadTimer;
        private double _LateCloseTimer;

        public Questions(Client client, Cell cell, Player thisPlayer)
        {
            _ThisPlayer = thisPlayer;
            _Client = client;
            _Cell = cell;

            InitializeComponent();
            GetQuestion();

            _Timer = new DispatcherTimer(DispatcherPriority.Normal);
            _Timer.Interval = TimeSpan.FromMilliseconds(1000);
            _Timer.Tick += new EventHandler(Timer_Tick_Event);
            _Timer.Start();

            _DeadTimer = 60;
            _LateCloseTimer = 3;
        }

        private void GetQuestion()
        {
            _Client.GetQuestionFromServer();
            _Question = _Client.GetQuestion();
            TextBlockQuestion.Text = _Question.TextQuestion;

            foreach (Button button in CanvasWithButtonsAnswers.Children)
            {
                int index = int.Parse((string)button.Tag);
                button.Content = _Question.ListAnswers[index];
            }
        }

        private void Timer_Tick_Event(object sender, EventArgs e)
        {
            Brush greenBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            _DeadTimer -= (int)_Timer.Interval.TotalSeconds;
            TextBlockTimer.Text = _DeadTimer.ToString();
            if (_DeadTimer <= 0)
            {
                _Timer.Stop();
                foreach (Button btn in CanvasWithButtonsAnswers.Children)
                {
                    if (_Client.SendAnswer((string)btn.Content))
                    {
                        btn.Background = greenBrush;

                        DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);
                        timer.Interval = TimeSpan.FromMilliseconds(500);
                        timer.Tick += new EventHandler(Tick_Event_Late_Close);
                        timer.Start();

                        break;
                    }
                }
            }
        }

        private void Button_Click_Event_EndQuestion(object sender, EventArgs e)
        {
            _Timer.Stop();

            var greenBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            var redBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            Button button = (Button)sender;
            int index = int.Parse((string)button.Tag);
            bool flag = _Client.SendAnswer(_Question.ListAnswers[index]);

            button.Background = flag ? greenBrush : redBrush;

            if (!flag)
            {
                foreach (Button btn in CanvasWithButtonsAnswers.Children)
                {
                    flag = _Client.SendAnswer((string)btn.Content);
                    if (flag)
                    {
                        btn.Background = greenBrush;
                        break;
                    }
                }
            }
            else
            {
                _Cell.OwnerId = _ThisPlayer.Id;
                _Client.UpdateCell(_Cell);
            }

            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(Tick_Event_Late_Close);
            timer.Start();
        }

        private void Tick_Event_Late_Close(object sender, EventArgs e)
        {
            foreach (Button button in CanvasWithButtonsAnswers.Children)
            {
                button.Click -= Button_Click_Event_EndQuestion;
            }

            DispatcherTimer timer = (DispatcherTimer)sender;
            _LateCloseTimer -= timer.Interval.TotalSeconds;
            if (_LateCloseTimer <= 0)
            {
                timer.Stop();
                WindowQuestions.Close();
            }
        }
    }
}