using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для SponsorsOverview.xaml
    /// </summary>
    public partial class SponsorsOverview : Page
    {
        public class CharitiesA
        {
            public int CharityId { get; set; }
            public string LogoName { get; set; }
            public BitmapImage Logo { get; set; }
            public string CharityName { get; set; }

            public decimal TotalAmount { get; set; }
            public int SponsorCount { get; set; }

            public CharitiesA(int charityId, string logoName, string name, int sponsorCount, decimal totalAmount)
            {
                CharityId = charityId;
                TotalAmount = totalAmount;
                SponsorCount = sponsorCount;
                LogoName = logoName;
                CharityName = name;
                Logo = new BitmapImage(new Uri($"C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Charity/{logoName}"));

            }
        }
        public SponsorsOverview()
        {
            InitializeComponent();
        }

        private void LoadSponsorsData(string sortBy = "TotalAmount DESC")
        {
            using (var db = new MarafonUchebkaEntities())
            {
                int currentMarathonId = GetCurrentMarathonId();
                var sponsorsQuery = from s in db.Sponsorship
                                    join r in db.Registration on s.RegistrationId equals r.RegistrationId
                                    join re in db.RegistrationEvent on r.RegistrationId equals re.RegistrationId
                                    join c in db.Charity on r.CharityId equals c.CharityId
                                    join e in db.Event on re.EventId equals e.EventId
                                    join m in db.Marathon on e.MarathonId equals m.MarathonId
                                    where m.MarathonId == currentMarathonId
                                    group s by new { c.CharityId, c.CharityName, c.CharityLogo } into g
                                    select new CharitySponsorshipInfo
                                    {
                                        CharityId = g.Key.CharityId,
                                        CharityName = g.Key.CharityName,
                                        Logo = g.Key.CharityLogo,
                                        TotalAmount = g.Sum(x => x.Amount),
                                        SponsorCount = g.Count()
                                    };

                // Применяем сортировку
                switch (sortBy)
                {
                    case "TotalAmount DESC":
                        sponsorsQuery = sponsorsQuery.OrderByDescending(x => x.TotalAmount);
                        break;
                    case "TotalAmount ASC":
                        sponsorsQuery = sponsorsQuery.OrderBy(x => x.TotalAmount);
                        break;
                    case "CharityName ASC":
                        sponsorsQuery = sponsorsQuery.OrderBy(x => x.CharityName);
                        break;
                    case "CharityName DESC":
                        sponsorsQuery = sponsorsQuery.OrderByDescending(x => x.CharityName);
                        break;
                }

                // Получаем данные с проверкой на null
                var sponsorsList = sponsorsQuery.ToList() ?? new List<CharitySponsorshipInfo>();

                // Обновляем UI с проверками
                if (txtCharityCount != null)
                {
                    txtCharityCount.Text = $"Благотворительные организации: {sponsorsList.Count}";
                }

                if (txtTotalDonations != null)
                {
                    decimal total = sponsorsList.Sum(x => x.TotalAmount);
                    txtTotalDonations.Text = $"Всего спонсорских взносов: {total:C}";
                }

                if (dgSponsors != null)
                {
                    ObservableCollection<CharitiesA> Abc = new ObservableCollection<CharitiesA>();
                    foreach (var c in sponsorsList)
                    {
                        Abc.Add(new CharitiesA(c.CharityId, c.Logo, c.CharityName, c.SponsorCount, c.TotalAmount));
                    }
                    dgSponsors.ItemsSource = Abc;
                }
            }
        }

        private int GetCurrentMarathonId()
        {
            // Здесь должна быть логика получения ID текущего марафона
            return 1; // Пример значения
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSortBy.SelectedItem == null) return;

            var sortOption = ((ComboBoxItem)cmbSortBy.SelectedItem).Content.ToString();
            string sortBy;
            switch (sortOption)
            {
                case "Итоговая сумма (по убыванию)":
                    sortBy = "TotalAmount DESC";
                    break;
                case "Итоговая сумма (по возрастанию)":
                    sortBy = "TotalAmount ASC";
                    break;
                case "Название организации (А-Я)":
                    sortBy = "CharityName ASC";
                    break;
                case "Название организации (Я-А)":
                    sortBy = "CharityName DESC";
                    break;
                default:
                    sortBy = "TotalAmount DESC";
                    break;
            }

            LoadSponsorsData(sortBy);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }

    public class CharitySponsorshipInfo
    {
        public int CharityId { get; set; }
        public string CharityName { get; set; }
        public string Logo { get; set; }
        public decimal TotalAmount { get; set; }
        public int SponsorCount { get; set; }
    }
}
