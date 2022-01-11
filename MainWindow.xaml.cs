using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using TriviadorClient.Entities;

namespace TriviadorClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void Delegate();
        private readonly Client _Client;
        private Thread _Thread;
        private int _Timer = 5;

        private string _nickName;

        public MainWindow()
        {
            _Client = Startup.GetClient();
            InitializeComponent();
        }

        private void Button_Click_HostGame(object sender, RoutedEventArgs e)
        {
            if (_Client.GetMap() != null)
            {
                TextBlockWrongNickName.Text = "Сервер уже запустился!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            string nickName = TextBoxNickName.Text;
            if (string.IsNullOrWhiteSpace(nickName))
            {
                TextBlockWrongNickName.Text = "Не подходящее имя!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            if (nickName.Length > 13)
            {
                TextBlockWrongNickName.Text = "Максимальная длина 13 символов!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            DispatcherTimer timer = new(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            _Thread = new Thread(delegate ()
            {
                Program.Start();
            });
            _Thread.Start();

            _nickName = nickName;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TextBlockWrongNickName.Visibility = Visibility.Visible;
            TextBlockWrongNickName.Text = _Timer--.ToString();
            if (_Timer <= 0)
            {
                DispatcherTimer timer = (DispatcherTimer)sender;
                timer.Stop();
                _Client.AddPlayer(TextBoxNickName.Text);

                WindowAuthorization.Visibility = Visibility.Hidden;
                new LoadingWindow(_Client, _nickName).Show();
                WindowAuthorization.Close();
            }
        }

        private void Button_Click_AddPlayer(object sender, RoutedEventArgs e)
        {
            string nickName = TextBoxNickName.Text;
            if (string.IsNullOrWhiteSpace(nickName))
            {
                TextBlockWrongNickName.Text = "Не подходящее имя!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            if (nickName.Length > 13)
            {
                TextBlockWrongNickName.Text = "Максимальная длина 13 символов!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            if (_Client.GetMap() == null)
            {
                TextBlockWrongNickName.Text = "Сервер еще не запустился!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            IEnumerable<string> names = from player in _Client.GetMap().Players select player.Name;

            if (names.Contains(nickName))
            {
                TextBlockWrongNickName.Text = "Такой ник уже существует!";
                TextBlockWrongNickName.Visibility = Visibility.Visible;
                return;
            }

            _Client.AddPlayer(TextBoxNickName.Text);
            WindowAuthorization.Visibility = Visibility.Hidden;
            new LoadingWindow(_Client, nickName).Show();
            WindowAuthorization.Close();
        }
    }
}