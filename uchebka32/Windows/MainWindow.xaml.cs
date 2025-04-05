using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using uchebka32.Pages;

namespace uchebka32.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime _targetDate;
        private DispatcherTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
            StartFrame.NavigationService.Navigate(new MainPage(MainFrame));

            _targetDate = new DateTime(2025, 12, 31, 23, 59, 59);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = _targetDate - DateTime.Now;

            TimerText.Text = string.Format("{0} дней {1} часов {2} минут до старта марафона!",
                remainingTime.Days,
                remainingTime.Hours,
                remainingTime.Minutes);

            if (remainingTime <= TimeSpan.Zero)
            {
                _timer.Stop();
                TimerText.Text = "Марафон начался!";
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.GoBack();
        }
    }
}
