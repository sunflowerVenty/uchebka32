using Microsoft.Win32;
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
        private string _photoPath;
        public RegRunner()
        {
            InitializeComponent();
            LoadGenders();
        }


        private void LoadGenders()
        {
            foreach (var item in ConnnectionDB.buEntities.Gender.ToList())
                cmbGender.Items.Add(item.Gender1.ToString());

            foreach (var item in ConnnectionDB.buEntities.Country.ToList())
                cmbCountry.Items.Add(item.CountryName.ToString());
        }
        

        private string _photoFilePath;

        private void NameTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-Я\s]+$"))
            {
                e.Handled = true;
            }
        }
        private void BtnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите фотографию бегуна",
                Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _photoFilePath = openFileDialog.FileName;

                    // Загружаем изображение для предпросмотра
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(_photoFilePath);
                    bitmap.DecodePixelWidth = 300; // Оптимизация для предпросмотра
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze(); // Для безопасности в многопоточной среде

                    imgRunnerPhoto.Source = bitmap;
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearPhoto();
                }
            }
        }

        private void BtnClearPhoto_Click(object sender, RoutedEventArgs e)
        {
            ClearPhoto();
        }

        private void ClearPhoto()
        {
            _photoFilePath = null;
            imgRunnerPhoto.Source = new BitmapImage(
                new Uri("pack://application:,,,/Resources/photo_placeholder.png"));
        }

        private byte[] SaveRunnerPhoto(string email)
        {
            if (string.IsNullOrEmpty(_photoFilePath))
                return null;

            try
            {
                // Читаем файл как массив байтов
                return File.ReadAllBytes(_photoFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить фотографию: {ex.Message}");
                return null;
            }
        }

        private bool ValidateForm()
        {
            // Проверка email
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Введите корректный email (формат: x@x.x)");
                return false;
            }

            // Проверка пароля
            var password = txtPassword.Text;
            if (password.Length < 6 ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsDigit) ||
                !password.Any(c => "!@#$%^".Contains(c)))
            {
                MessageBox.Show("Пароль должен содержать:\n- минимум 6 символов\n- минимум 1 заглавную букву\n- минимум 1 цифру\n- один из символов: !@#$%^");
                return false;
            }

            if (password != txtConfirmPassword.Text)
            {
                MessageBox.Show("Пароли не совпадают");
                return false;
            }

            // Проверка имени и фамилии
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Заполните имя и фамилию");
                return false;
            }

            // Проверка даты рождения
            if (dpBirthDate.SelectedDate == null ||
                dpBirthDate.SelectedDate > DateTime.Now.AddYears(-10))
            {
                MessageBox.Show("Бегун должен быть старше 10 лет");
                return false;
            }

            // Если все проверки пройдены
            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm()) return;

            //try
            //{
            using (var db = new MarafonUchebkaEntities())
            {
                // Проверка существующего email
                if (db.User.Any(u => u.Email == txtEmail.Text))
                {
                    MessageBox.Show("Пользователь с таким email уже существует");
                    return;
                }

                // Создание пользователя
                var user = new User
                {
                    Email = txtEmail.Text,
                    Password = txtPassword.Text, // Не забудьте реализовать хеширование
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    RoleId = db.Role.First(r => r.RoleName == "Runner").RoleId

                };

                db.User.Add(user);
                db.SaveChanges();

                // Создание бегуна
                var c = ConnnectionDB.buEntities.Gender.FirstOrDefault(a => a.Gender1 == cmbGender.Text);
                var d = ConnnectionDB.buEntities.Country.FirstOrDefault(a => a.CountryName == cmbCountry.Text);
                var runner = new Runner
                {
                    Email = txtEmail.Text, // Связь по email
                    Gender = c.Gender1, // Или другой способ получения значения
                    DateOfBirth = dpBirthDate.SelectedDate.Value,
                    CountryCode = d.CountryCode // Предполагая, что это код страны     
                };

                if (!string.IsNullOrEmpty(_photoFilePath))
                {
                    runner.PhotoPath = SaveRunnerPhoto(user.Email);
                }

                db.Runner.Add(runner);
                db.SaveChanges();

                MessageBox.Show("Регистрация успешно завершена!");
                NavigationService.Navigate(new RunnerRegistrationConfirmation());
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            //}
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }


    }
}