using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Data.Entity;
using uchebka32.Database;

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для RunnerEditProfile.xaml
    /// </summary>
    public partial class RunnerEditProfile : Page
    {
        private string currentUserEmail;
        public RunnerEditProfile(string email)
        {
            InitializeComponent();
            currentUserEmail = email;
            this.Loaded += EditProfilePage_Loaded;
        }

        private void BrowsePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите фотографию"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    PhotoPathTextBox.Text = dlg.FileName;
                    RunnerImage.Source = new BitmapImage(new Uri(dlg.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void EditProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGenders();
            LoadCountries();
            LoadRunnerData();
        }

        private void LoadGenders()
        {
            using (var context = new MarafonUchebkaEntities())
            {
                GenderComboBox.ItemsSource = context.Gender.ToList();
                GenderComboBox.DisplayMemberPath = "Gender1";
                GenderComboBox.SelectedValuePath = "Gender1";
            }
        }

        private void LoadCountries()
        {
            using (var context = new MarafonUchebkaEntities())
            {
                CountryComboBox.ItemsSource = context.Country.ToList();
                CountryComboBox.DisplayMemberPath = "CountryName";
                CountryComboBox.SelectedValuePath = "CountryCode";
            }
        }

        private void LoadRunnerData()
        {
            using (var context = new MarafonUchebkaEntities())
            {
                var runner = context.Runner
                    .Include(r => r.User)
                    .FirstOrDefault(r => r.Email == currentUserEmail);

                if (runner != null)
                {
                    EmailTextBlock.Text = runner.Email;
                    FirstNameTextBox.Text = runner.User.FirstName;
                    LastNameTextBox.Text = runner.User.LastName;
                    GenderComboBox.SelectedValue = runner.Gender;
                    CountryComboBox.SelectedValue = runner.CountryCode;
                    BirthDatePicker.SelectedDate = runner.DateOfBirth;

                    if (runner.PhotoPath != null && runner.PhotoPath.Length > 0)
                    {
                        try
                        {
                            var image = new BitmapImage();
                            using (var mem = new MemoryStream(runner.PhotoPath))
                            {
                                mem.Position = 0;
                                image.BeginInit();
                                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.UriSource = null;
                                image.StreamSource = mem;
                                image.EndInit();
                            }
                            image.Freeze();
                            RunnerImage.Source = image;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка загрузки фото: {ex.Message}",
                                          "Ошибка",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => "!@#$%^".Contains(ch));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                GenderComboBox.SelectedItem == null ||
                CountryComboBox.SelectedItem == null ||
                BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            var birthDate = BirthDatePicker.SelectedDate.Value;
            var age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;

            if (age < 10)
            {
                MessageBox.Show("Вам должно быть не менее 10 лет.",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            string password = PassTextBox.Password;
            string confirmPassword = RePassTextBox.Password;

            if (!string.IsNullOrWhiteSpace(password))
            {
                if (password != confirmPassword)
                {
                    MessageBox.Show("Пароль и подтверждение пароля не совпадают.",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                if (!IsValidPassword(password))
                {
                    MessageBox.Show("Пароль должен содержать минимум 6 символов, 1 заглавную букву, 1 цифру и один из символов: ! @ # $ % ^",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }
            }

            using (var context = new MarafonUchebkaEntities())
            {
                var runner = context.Runner
                    .Include(r => r.User)
                    .FirstOrDefault(r => r.Email == currentUserEmail);

                if (runner != null)
                {
                    runner.User.FirstName = FirstNameTextBox.Text.Trim();
                    runner.User.LastName = LastNameTextBox.Text.Trim();

                    if (!string.IsNullOrWhiteSpace(password))
                        runner.User.Password = password;

                    runner.Gender = GenderComboBox.SelectedValue.ToString();
                    runner.CountryCode = CountryComboBox.SelectedValue.ToString();
                    runner.DateOfBirth = birthDate;

                    string selectedPhotoPath = PhotoPathTextBox.Text;
                    if (!string.IsNullOrEmpty(selectedPhotoPath) && File.Exists(selectedPhotoPath))
                    {
                        try
                        {
                            byte[] imageData = File.ReadAllBytes(selectedPhotoPath);
                            runner.PhotoPath = imageData;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при чтении фото: {ex.Message}",
                                          "Ошибка",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                            return;
                        }
                    }

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show("Профиль успешно обновлён!",
                                      "Успех",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

    }
}
