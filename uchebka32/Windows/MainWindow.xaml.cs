using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using uchebka32.Database;
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
            MainFrame.Navigated += MainFrame_Navigated;
            UpdateBackButtonVisibility();

            StartFrame.NavigationService.Navigate(new MainPage(MainFrame));

            _targetDate = new DateTime(2025, 12, 31, 23, 59, 59);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateBackButtonVisibility();
        }

        private void UpdateBackButtonVisibility()
        {
            BackBtn.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;

            if (ConnnectionDB.user == null) LogoutBtn.Visibility = Visibility.Hidden;
            else LogoutBtn.Visibility = Visibility.Visible;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
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

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnnectionDB.user = null;
            StartFrame.NavigationService.Navigate(new MainPage(MainFrame));
        }
    }
}
