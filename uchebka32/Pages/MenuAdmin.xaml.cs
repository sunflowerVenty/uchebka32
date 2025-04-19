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
    /// Логика взаимодействия для MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Page
    {
        public MenuAdmin()
        {
            InitializeComponent();
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            //    NavigationService?.Navigate(new UsersManagementPage());
        }

        private void VolunteersButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Volonteers());
        }

        private void CharitiesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EditCharity());
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new InventoryManagementPage());
        }
    }
}
