using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using uchebka32.Database;
using uchebka32.Windows;

namespace uchebka32.Pages
{
    public partial class RegSpons : Page
    {
        private int donationAmount = 0;
        private int currentCharityId = 1;

        public RegSpons()
        {
            InitializeComponent();
            InitializePlaceholders();
            LoadRunners();
            UpdateDonationDisplay();
            LoadCharityInfo();
        }

        private void InitializePlaceholders()
        {
            SetPlaceholder(NameTextBox, "Ваше имя");
            SetPlaceholder(CardHolderTextBox, "Владелец карты");
            SetPlaceholder(CardNumberTextBox, "1234 1234 1234 1234");
            SetPlaceholder(ExpiryMonthTextBox, "MM");
            SetPlaceholder(ExpiryYearTextBox, "YYYY");
            SetPlaceholder(CVCTextBox, "CVC");
            SetPlaceholder(DonationTextBox, "0");
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Foreground = Brushes.Gray;
                    switch (textBox.Name)
                    {
                        case "NameTextBox": textBox.Text = "Ваше имя"; break;
                        case "CardHolderTextBox": textBox.Text = "Владелец карты"; break;
                        case "CardNumberTextBox": textBox.Text = "1234 1234 1234 1234"; break;
                        case "ExpiryMonthTextBox": textBox.Text = "MM"; break;
                        case "ExpiryYearTextBox": textBox.Text = "YYYY"; break;
                        case "CVCTextBox": textBox.Text = "CVC"; break;
                        case "DonationTextBox": textBox.Text = "0"; break;
                    }
                }
            }
        }

        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            string text = textBox.Text.Replace(" ", "");

            if (text.Length > 16) text = text.Substring(0, 16);

            string formatted = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && i % 4 == 0) formatted += " ";
                formatted += text[i];
            }

            textBox.Text = formatted;
            textBox.CaretIndex = formatted.Length;
        }

        private void LoadRunners()
        {
            try
            {
                var runners = (from r in ConnnectionDB.buEntities.Runner
                               join u in ConnnectionDB.buEntities.User on r.Email equals u.Email
                               join reg in ConnnectionDB.buEntities.Registration on r.RunnerId equals reg.RunnerId
                               join re in ConnnectionDB.buEntities.RegistrationEvent on reg.RegistrationId equals re.RegistrationId
                               where reg.RegistrationStatusId == 2
                               select new
                               {
                                   u.FirstName,
                                   u.LastName,
                                   re.BibNumber,
                                   r.CountryCode
                               }).ToList();

                Console.WriteLine($"Найдено бегунов: {runners.Count}");

                if (runners.Any())
                {
                    RunnerComboBox.ItemsSource = runners.Select(r =>
                        $"{r.FirstName} {r.LastName} - {r.BibNumber} ({r.CountryCode})").ToList();

                    if (RunnerComboBox.Items.Count > 0)
                        Console.WriteLine($"Первый бегун: {RunnerComboBox.Items[0]}");
                }
                else
                {
                    MessageBox.Show("Нет зарегистрированных бегунов. Пожалуйста, попробуйте позже.",
                                  "Информация",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списка бегунов: {ex.Message}\n\nПодробности:\n{ex.InnerException?.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);

                Console.WriteLine($"Ошибка: {ex}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException}");
            }
        }

        private void LoadCharityInfo()
        {
            try
            {
                var charity = ConnnectionDB.buEntities.Charity
                    .FirstOrDefault(c => c.CharityId == currentCharityId);

                if (charity != null)
                {
                    CharityNameTextBlock.Text = charity.CharityName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки информации о благотворительности: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void UpdateDonationDisplay()
        {
            DonationTextBlock.Text = $"${donationAmount}";
            DonationTextBox.Text = donationAmount.ToString();
        }

        private void IncreaseDonation_Click(object sender, RoutedEventArgs e)
        {
            donationAmount += 10;
            UpdateDonationDisplay();
        }

        private void DecreaseDonation_Click(object sender, RoutedEventArgs e)
        {
            if (donationAmount >= 10)
            {
                donationAmount -= 10;
                UpdateDonationDisplay();
            }
        }

        private void DonationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DonationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(DonationTextBox.Text, out int amount))
            {
                donationAmount = amount;
                UpdateDonationDisplay();
            }
            else if (string.IsNullOrWhiteSpace(DonationTextBox.Text))
            {
                donationAmount = 0;
                UpdateDonationDisplay();
            }
        }

        private bool ValidateCardData()
        {
            string cardNumber = CardNumberTextBox.Text.Replace(" ", "");
            if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
            {
                MessageBox.Show("Номер карты должен содержать 16 цифр",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(ExpiryMonthTextBox.Text, out int month) ||
                !int.TryParse(ExpiryYearTextBox.Text, out int year) ||
                month < 1 || month > 12)
            {
                MessageBox.Show("Некорректный срок действия карты. Используйте формат MM/YYYY",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return false;
            }

            DateTime now = DateTime.Now;
            DateTime expiryDate = new DateTime(year, month, 1).AddMonths(1);
            if (expiryDate < now)
            {
                MessageBox.Show("Срок действия карты истек",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return false;
            }

            if (CVCTextBox.Text.Length != 3 || !CVCTextBox.Text.All(char.IsDigit))
            {
                MessageBox.Show("CVC код должен содержать 3 цифры",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) || NameTextBox.Foreground == Brushes.Gray)
                {
                    MessageBox.Show("Пожалуйста, введите ваше имя",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                if (RunnerComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите бегуна",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(CardHolderTextBox.Text) || CardHolderTextBox.Foreground == Brushes.Gray)
                {
                    MessageBox.Show("Пожалуйста, введите карту",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(CardNumberTextBox.Text) || CardNumberTextBox.Foreground == Brushes.Gray)
                {
                    MessageBox.Show("Пожалуйста, номер карты",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExpiryMonthTextBox.Text) || ExpiryMonthTextBox.Foreground == Brushes.Gray)
                {
                    MessageBox.Show("Пожалуйста, введите месяц срока карты",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExpiryYearTextBox.Text) || ExpiryYearTextBox.Foreground == Brushes.Gray)
                {
                    MessageBox.Show("Пожалуйста, введите год срока карты",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(CVCTextBox.Text) || ExpiryYearTextBox.Foreground == Brushes.Gray)
                {
                    MessageBox.Show("Пожалуйста, введите CVC код",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }
                
                


                if (!ValidateCardData())
                {
                    return;
                }

                if (donationAmount <= 0)
                {
                    MessageBox.Show("Сумма пожертвования должна быть больше 0",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                string selectedRunner = RunnerComboBox.SelectedItem.ToString();
                int bibNumber = int.Parse(selectedRunner.Split('-')[1].Trim().Split(' ')[0]);

                var registration = (from re in ConnnectionDB.buEntities.RegistrationEvent
                                    where re.BibNumber == bibNumber
                                    select re.Registration).FirstOrDefault();

                if (registration != null)
                {
                    var sponsorship = new Sponsorship
                    {
                        SponsorName = NameTextBox.Text,
                        RegistrationId = registration.RegistrationId,
                        Amount = donationAmount
                    };

                    ConnnectionDB.buEntities.Sponsorship.Add(sponsorship);
                    ConnnectionDB.buEntities.SaveChanges();

                    NavigationService.Navigate(new thankSpons(RunnerComboBox.Text, bibNumber.ToString(), sponsorship.SponsorName, donationAmount));

                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Не удалось найти регистрацию бегуна",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            NameTextBox.Text = "Ваше имя";
            NameTextBox.Foreground = Brushes.Gray;

            RunnerComboBox.SelectedItem = null;

            CardHolderTextBox.Text = "Владелец карты";
            CardHolderTextBox.Foreground = Brushes.Gray;

            CardNumberTextBox.Text = "1234 1234 1234 1234";
            CardNumberTextBox.Foreground = Brushes.Gray;

            ExpiryMonthTextBox.Text = "MM";
            ExpiryMonthTextBox.Foreground = Brushes.Gray;

            ExpiryYearTextBox.Text = "YYYY";
            ExpiryYearTextBox.Foreground = Brushes.Gray;

            CVCTextBox.Text = "CVC";
            CVCTextBox.Foreground = Brushes.Gray;

            donationAmount = 0;
            UpdateDonationDisplay();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                NavigationService.Navigate(new InfoMarafon());
            }
        }

        private void CharityInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var charity = ConnnectionDB.buEntities.Charity
                    .FirstOrDefault(c => c.CharityId == currentCharityId);

                if (charity != null)
                {
                    MessageBox.Show(charity.CharityDescription,
                                  $"Информация о {charity.CharityName}",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке информации: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void NameTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-Я\s]+$"))
            {
                e.Handled = true;
            }
        }

        private void CardNumberTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^\d+$"))
            {
                e.Handled = true;
            }
        }

        private void ExpiryMonthTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^\d+$"))
            {
                e.Handled = true;
            }
        }

        private void ExpiryYearTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^\d+$"))
            {
                e.Handled = true;
            }
        }

        private void CVCTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^\d+$"))
            {
                e.Handled = true;
            }
        }
    }
}