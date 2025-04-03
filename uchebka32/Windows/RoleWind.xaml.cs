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
using System.Windows.Shapes;

namespace uchebka32.Windows
{
    /// <summary>
    /// Логика взаимодействия для RolePage.xaml
    /// </summary>
    public partial class RoleWind : Window
    {
        public string role;
        public RoleWind()
        {
            InitializeComponent();
        }

        private void RunnerBtn_Click(object sender, RoutedEventArgs e)
        {
            role = RunnerBtn.Content.ToString();
            this.Close();
        }

        private void KoorBtn_Click(object sender, RoutedEventArgs e)
        {
            role = KoorBtn.Content.ToString();
            this.Close();
        }

        private void AdminBtn_Click(object sender, RoutedEventArgs e)
        {
            role = AdminBtn.Content.ToString();
            this.Close();
        }
    }
}
