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
    /// Логика взаимодействия для RegRunner.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public string Role;
        public AuthPage()
        {
            InitializeComponent();
            RoleWind roleWind = new RoleWind();
            bool? result = roleWind.ShowDialog();

            if(result == true)
            {
                Role = roleWind.role;
            }
        }
    }
}
