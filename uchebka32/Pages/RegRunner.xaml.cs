using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using uchebka32.Database;

namespace uchebka32.Pages
{
    public partial class RegRunner : Page
    {
        public RegRunner()
        {
            InitializeComponent();
            LoadGenders();
            LoadCountries();
        }
        private void LoadGenders()
        {
            foreach (var item in ConnnectionDB.buEntities.Gender.ToList())
            {
                GenderComboBox.Items.Add(item.Gender1.ToString());
            }
        }
        private void LoadCountries()
        {
            foreach (var item in ConnnectionDB.buEntities.Country.ToList())
            {
                CountryComboBox.Items.Add(item.CountryName.ToString());
            }
        }

        private void BrowsePhoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png"
            };
            if (dialog.ShowDialog() == true)
            {
                PhotoPathTextBox.Text = dialog.FileName;
                BgPhotoPanel.Background = null;
                RunnerPhotoImage.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }

        private void BirthDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthDatePicker.SelectedDate.HasValue)
            {
                var age = DateTime.Now.Year - BirthDatePicker.SelectedDate.Value.Year;
                if (age < 10)
                {
                    MessageBox.Show("Бегуну должно быть не менее 10 лет.");
                    BirthDatePicker.SelectedDate = null;
                }
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Text) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                GenderComboBox.SelectedItem == null ||
                CountryComboBox.SelectedItem == null ||
                BirthDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(PhotoPathTextBox.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения.");
                return;
            }

            if (PasswordTextBox.Text != ConfirmPasswordTextBox.Text)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            MessageBox.Show("Регистрация успешно завершена!");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Email" ||
                    textBox.Text == "Пароль" ||
                    textBox.Text == "Повторите пароль" ||
                    textBox.Text == "Имя" ||
                    textBox.Text == "Фамилия")
                {
                    textBox.Text = "";
                }
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                switch (textBox.Name)
                {
                    case "EmailTextBox":
                        string email = EmailTextBox.Text;
                        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                        {
                            MessageBox.Show("Некорректный формат Email.");
                        }
                        break;
                    case "PasswordTextBox":
                        textBox.Text = "Пароль";
                        break;
                    case "ConfirmPasswordTextBox":
                        textBox.Text = "Повторите пароль";
                        break;
                    case "FirstNameTextBox":
                        textBox.Text = "Имя";
                        break;
                    case "LastNameTextBox":
                        textBox.Text = "Фамилия";
                        break;

                    default:
                        break;
                }
                
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    switch (textBox.Name)
                    {
                        case "EmailTextBox":
                            textBox.Text = "Email";
                            break;
                        case "PasswordTextBox":
                            string password = PasswordTextBox.Text;
                            if (password.Length < 6 || !password.Any(char.IsUpper) || !password.Any(char.IsDigit) ||
                                !password.Any(c => "!@#$%^".Contains(c)))
                            {
                                MessageBox.Show("Пароль не соответствует требованиям.");
                            }
                            break;
                        case "ConfirmPasswordTextBox":
                            if (PasswordTextBox.Text != ConfirmPasswordTextBox.Text)
                            {
                                MessageBox.Show("Пароли не совпадают.");
                            }
                            break;
                        case "FirstNameTextBox":
                            if (!Regex.IsMatch(textBox.Text, @"^[a-zA-Zа-яА-Я\s]+$"))
                            {
                                e.Handled = true;
                            }
                            break;
                        case "LastNameTextBox":
                            if (!Regex.IsMatch(textBox.Text, @"^[a-zA-Zа-яА-Я\s]+$"))
                            {
                                e.Handled = true;
                            }
                            break;

                        default:
                            break;
                    }
                    textBox.Foreground = new SolidColorBrush(Colors.LightGray);
                }
            }
        }
    }
}