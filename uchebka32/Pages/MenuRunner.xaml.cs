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
    /// Логика взаимодействия для MenuRunner.xaml
    /// </summary>
    public partial class MenuRunner : Page
    {
        public MenuRunner()
        {
            InitializeComponent();
        }

        private void Contacts_Click(object sender, RoutedEventArgs e)
        {
            ContactOrgWind contactOrgWind = new ContactOrgWind();
            contactOrgWind.Show();
        }
    }
}
