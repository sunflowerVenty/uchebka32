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
    /// Логика взаимодействия для RegRunner2Page.xaml
    /// </summary>
    public partial class RegRunner2 : Page
    {
        private decimal _donationAmount = 500;
        private Charity _selectedCharity;
        public RegRunner2()
        {
            try
            {
                InitializeComponent();
                LoadCharities();
                CalculateCost(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации страницы: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService?.GoBack();
            }
        }

        private void LoadCharities()
        {
            try
            {
                using (var db = new MarafonUchebkaEntities())
                {
                    cmbCharity.ItemsSource = db.Charity
                        .OrderBy(c => c.CharityName)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списка благотворительных фондов: {ex.Message}");
            }
        }

        private void CalculateCost(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal totalCost = 0;

                if (chk5km?.IsChecked == true) totalCost += 20;
                if (chk21km?.IsChecked == true) totalCost += 75;
                if (chk42km?.IsChecked == true) totalCost += 145;

                if (rbKitB?.IsChecked == true)
                    totalCost += 20;
                else if (rbKitC?.IsChecked == true)
                    totalCost += 45;

                if (txtCost != null)
                    txtCost.Text = $"${totalCost}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета стоимости: {ex.Message}");
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.Text, e.Text.Length - 1))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка валидации ввода: {ex.Message}");
            }
        }

        private void txtDonation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (decimal.TryParse(txtDonation?.Text, out decimal amount))
                {
                    _donationAmount = amount;
                }
                else if (txtDonation != null)
                {
                    txtDonation.Text = _donationAmount.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обработки суммы взноса: {ex.Message}");
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedCharity == null)
                {
                    MessageBox.Show("Выберите благотворительную организацию",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string charityInfo = $"{_selectedCharity.CharityName}\n\n{_selectedCharity.CharityDescription}";

                MessageBox.Show(charityInfo, "Информация о благотворительности",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения информации: {ex.Message}");
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка выбора марафона
                bool anyMarathonSelected = (chk5km?.IsChecked == true) ||
                                         (chk21km?.IsChecked == true) ||
                                         (chk42km?.IsChecked == true);

                if (!anyMarathonSelected)
                {
                    MessageBox.Show("Пожалуйста, выберите хотя бы один вид марафона",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка суммы взноса
                if (_donationAmount <= 0)
                {
                    MessageBox.Show("Сумма взноса должна быть положительной",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка выбора благотворительной организации
                if (_selectedCharity == null)
                {
                    MessageBox.Show("Пожалуйста, выберите благотворительную организацию",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Сохранение данных в БД
                using (var db = new MarafonUchebkaEntities())
                {
                    // Находим RunnerId по Email текущего пользователя
                    var runner = db.Runner.FirstOrDefault(r => r.Email == App.CurrentUser.Email);

                    if (runner == null)
                    {
                        MessageBox.Show("Профиль бегуна не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Теперь используем runner.RunnerId
                    var registration = new Registration()
                    {
                        RunnerId = runner.RunnerId, // Теперь правильно
                        RegistrationDateTime = DateTime.Now,
                        RaceKitOptionId = rbKitB.IsChecked == true ? "B" : "C",
                        RegistrationStatusId = 1,
                        Cost = decimal.Parse(txtCost.Text.Replace("$", "")),
                        CharityId = _selectedCharity.CharityId,
                        SponsorshipTarget = _donationAmount
                    };

                    db.Registration.Add(registration);
                    db.SaveChanges();

                    // Получаем ID созданной регистрации
                    int registrationId = registration.RegistrationId;

                    // Создаем записи в таблице RegistrationEvent для каждого выбранного марафона
                    if (chk5km.IsChecked == true)
                    {
                        db.RegistrationEvent.Add(new RegistrationEvent()
                        {
                            RegistrationId = registrationId,
                            EventId = "15_5FM", // Или соответствующий ID из таблицы Events
                            BibNumber = GenerateBibNumber(),
                            RaceTime = null // Время будет заполнено после забега
                        });
                    }

                    if (chk21km.IsChecked == true)
                    {
                        db.RegistrationEvent.Add(new RegistrationEvent()
                        {
                            RegistrationId = registrationId,
                            EventId = "15_5FR",
                            BibNumber = GenerateBibNumber(),
                            RaceTime = null
                        });
                    }

                    if (chk42km.IsChecked == true)
                    {
                        db.RegistrationEvent.Add(new RegistrationEvent()
                        {
                            RegistrationId = registrationId,
                            EventId = "15_5HM",
                            BibNumber = GenerateBibNumber(),
                            RaceTime = null
                        });
                    }

                    db.SaveChanges();
                }

                MessageBox.Show("Регистрация успешно завершена!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService?.Navigate(new RunnerRegistrationConfirmation());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }

        private short GenerateBibNumber()
        {
            Random random = new Random();
            return (short)random.Next(1000, 9999); // Генерируем 4-значный номер
        }

        private string GetSelectedRaceType()
        {
            if (chk5km.IsChecked == true) return "5km";
            if (chk21km.IsChecked == true) return "21km";
            if (chk42km.IsChecked == true) return "42km";
            return "";
        }

        private void cmbCharity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCharity.SelectedItem is Charity selectedCharity)
                {
                    _selectedCharity = selectedCharity;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора организации: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

    }
}
