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

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для Information.xaml
    /// </summary>
    public partial class DeteilsInfo : Page
    {
        public DeteilsInfo()
        {
            InitializeComponent();
        }

        private void CharityBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListCharity());
        }


        private void MarathonDuration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MarathonDuration());
        }

        private void PreviousResults_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PreviosPaseResults());
        }

        private void BMICalculator_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BMICalculator());
        }

        private void MarathonSkillsInfo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InfoMarafon());
        }

        private void BMRCalculator_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BMRCalculator());
        }

    }
}
