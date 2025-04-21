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
using uchebka32.Database;

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для thankSpons.xaml
    /// </summary>
    public partial class thankSpons : Page
    {
        public thankSpons(string runnerName, string bibNumber, string charityName, decimal amount)
        {
            InitializeComponent();
            RunnerInfoTextBlock.Text = $"{runnerName} ({bibNumber})";
            CharityTextBlock.Text = charityName;
            AmountTextBlock.Text = $"${amount}";
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
