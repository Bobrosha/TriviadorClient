using System;
using System.Windows;
using System.Windows.Threading;
using TriviadorClient.Entities;

namespace TriviadorClient
{
    /// <summary>
    /// Логика взаимодействия для Questions.xaml
    /// </summary>
    public partial class Questions : Window
    {
        private readonly Client _Client;
        private DispatcherTimer _Timer;
        private int _DeadTimer;
        public Questions(Client client)
        {
            _Client = client;
            InitializeComponent();
            _Timer = new DispatcherTimer(DispatcherPriority.Normal);
            _Timer.Interval = TimeSpan.FromMilliseconds(1000);
            _Timer.Tick += new EventHandler(Timer_Tick);
            _Timer.Start();
            _DeadTimer = 60;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _DeadTimer -= (int)_Timer.Interval.TotalSeconds;
            TextBlockTimer.Text = _DeadTimer.ToString();
            if (_DeadTimer <= 0)
            {
                _Timer.Stop();
                WindowQuestions.Close();
            }
        }

        private void Button_Click_EndQuestion(object sender, EventArgs e)
        {
            WindowQuestions.Close();
        }
    }
}