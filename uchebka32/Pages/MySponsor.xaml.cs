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
    /// Логика взаимодействия для MySponsor.xaml
    /// </summary>
    public partial class MySponsor : Page
    {
        private readonly MarafonUchebkaEntities _dbContext;
        private readonly int _runnerId;
        public MySponsor(int runnerId)
        {
            InitializeComponent();
            _dbContext = new MarafonUchebkaEntities();
            _runnerId = runnerId;
        }

        private void LoadSponsorshipData()
        {
            try
            {
                // Получаем регистрацию бегуна (без проверки года марафона)
                var registration = _dbContext.Registration
                    .FirstOrDefault(r => r.RunnerId == _runnerId);

                if (registration == null)
                {
                    ShowNoRegistrationMessage();
                    return;
                }

                // Получаем информацию о благотворительной организации
                var charity = registration.Charity;
                if (charity != null)
                {
                    DisplayCharityInfo(charity);
                }

                // Получаем список спонсоров
                var sponsorships = _dbContext.Sponsorship
                    .Where(s => s.RegistrationId == registration.RegistrationId)
                    .ToList();

                DisplaySponsorships(sponsorships);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowNoRegistrationMessage()
        {
            CharityPanel.Visibility = Visibility.Collapsed;
            SponsorsPanel.Visibility = Visibility.Collapsed;

            var messageText = new TextBlock
            {
                Text = "Вы не зарегистрированы на Marathon Skills 2025 или у вас нет спонсоров.",
                FontSize = 16,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };

            MainStackPanel.Children.Add(messageText);
        }

        private void DisplayCharityInfo(uchebka32.Database.Charity charity)
        {
            CharityNameTextBlock.Text = charity.CharityName;
            CharityDescriptionTextBlock.Text = string.IsNullOrWhiteSpace(charity.CharityDescription)
                ? "Описание отсутствует"
                : charity.CharityDescription;

            string logoFileName = string.IsNullOrEmpty(charity.CharityLogo)
                ? "default_logo.png"
                : charity.CharityLogo;

            string imagePath = $"pack://application:,,,/Images/{logoFileName}";

            try
            {
                // Загрузка изображения
                var bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

                // Устанавливаем ImageBrush
                var imageBrush = new ImageBrush
                {
                    ImageSource = bitmapImage,
                    Stretch = Stretch.UniformToFill,  // Масштабируем изображение, чтобы оно заполнило контейнер
                    AlignmentX = AlignmentX.Center,
                    AlignmentY = AlignmentY.Center
                };

                // Применяем clip для круглой формы
                var ellipseGeometry = new EllipseGeometry
                {
                    Center = new Point(LogoGrid.Width / 2, LogoGrid.Height / 2),
                    RadiusX = LogoGrid.Width / 2,
                    RadiusY = LogoGrid.Height / 2
                };

                LogoGrid.Clip = ellipseGeometry;
                LogoImageBrush.ImageSource = imageBrush.ImageSource;
            }
            catch
            {
                // Фолбэк, если изображение не найдено
                LogoImageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/default_logo.png", UriKind.Absolute));
            }
        }



        private void DisplaySponsorships(List<Sponsorship> sponsorships)
        {
            SponsorsGrid.Children.Clear();

            if (sponsorships.Count == 0)
            {
                var noSponsorsText = new TextBlock
                {
                    Text = "У вас пока нет спонсоров.",
                    FontSize = 14,
                    Foreground = Brushes.LightGray,
                    Margin = new Thickness(0, 10, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                SponsorsListPanel.Children.Add(noSponsorsText);
                TotalAmountTextBlock.Text = "Всего $0";
                return;
            }

            decimal totalAmount = 0;

            foreach (var sponsorship in sponsorships)
            {
                var sponsorNameText = new TextBlock
                {
                    Text = sponsorship.SponsorName,
                    Foreground = Brushes.LightGray
                };

                var amountText = new TextBlock
                {
                    Text = $"{sponsorship.Amount:C}",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Foreground = Brushes.LightGray
                };

                SponsorsGrid.Children.Add(sponsorNameText);
                SponsorsGrid.Children.Add(amountText);

                totalAmount += sponsorship.Amount;
            }

            TotalAmountTextBlock.Text = $"Всего {totalAmount:C}";
        }

    }
}
