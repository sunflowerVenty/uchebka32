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
using System.Windows.Navigation;
using System.Windows.Shapes;
using uchebka32.Windows;

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Frame frame;
        public MainPage(Frame _frame)
        {
            InitializeComponent();
            frame = _frame;

        }

        private void RunnerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            frame.NavigationService.Navigate(new CheckRunner());
        }

        private void SponsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            frame.NavigationService.Navigate(new RegSpons());
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            frame.NavigationService.Navigate(new DeteilsInfo());
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            frame.NavigationService.Navigate(new AuthPage());
        }
    }
}
