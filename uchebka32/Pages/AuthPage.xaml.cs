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
using uchebka32.Database;

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
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(EmailBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите email!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (string.IsNullOrEmpty(PassBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var us = ConnnectionDB.buEntities.User.Where(email => email.Email == EmailBox.Text).FirstOrDefault();
                    if (us != null)
                    {
                        if (us.Password == PassBox.Text)
                        {
                            ConnnectionDB.user = us;
                            if (us.RoleId == "R") NavigationService.Navigate(new MenuRunner());
                            else if (us.RoleId == "C") NavigationService.Navigate(new MenuKoor());
                            else if (us.RoleId == "A") { } /*NavigationService.Navigate();*/
                        }
                    }
                    else if (ConnnectionDB.user == null)
                    {
                        MessageBox.Show("Неверные имя пользователя или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); 
                    }
                    

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void DopBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegRunner());
        }
    }
}
