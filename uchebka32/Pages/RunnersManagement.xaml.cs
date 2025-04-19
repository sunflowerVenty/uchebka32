using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для RunnersManagement.xaml
    /// </summary>
    /// 
    public class RunnerInfoDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Charity { get; set; }
        public decimal SponsorshipTarget { get; set; }
        public byte RegistrationStatusId { get; set; }

        // Дополнительные свойства для удобства
        public string GenderDisplay => Gender == "M" ? "мужской" : "женский";
        public string BirthDateDisplay => DateOfBirth?.ToString("dd MMMM yyyy") ?? "не указана";
        public string DonationDisplay => SponsorshipTarget.ToString("C");
    }
    public partial class RunnersManagement : Page
    {
        private MarafonUchebkaEntities dbContext = ConnnectionDB.buEntities;
        public RunnersManagement()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += RunnersManagementPage_Loaded;
        }


        private void RunnersManagementPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCmb();
            LoadRunners();
        }

        private int _selectedStatusId;
        public int SelectedStatusId
        {
            get => _selectedStatusId;
            set { _selectedStatusId = value; OnPropertyChanged(); LoadRunners(); }
        }

        private int _selectedDistanceId;
        public int SelectedDistanceId
        {
            get => _selectedDistanceId;
            set { _selectedDistanceId = value; OnPropertyChanged(); LoadRunners(); }
        }

        private string _selectedSortField;
        public string SelectedSortField
        {
            get => _selectedSortField;
            set { _selectedSortField = value; OnPropertyChanged(); LoadRunners(); }
        }

        public ObservableCollection<RunnerInfo> Runners { get; set; } = new ObservableCollection<RunnerInfo>();
        public ObservableCollection<RegistrationStatus> Statuses { get; set; } = new ObservableCollection<RegistrationStatus>();
        public ObservableCollection<Distance> Distances { get; set; } = new ObservableCollection<Distance>();
        public ObservableCollection<SortOption> SortOptions { get; set; } = new ObservableCollection<SortOption>();

        private void LoadCmb()
        {
            try
            {
                var statuses = dbContext.RegistrationStatus
                .Select(s => new { s.RegistrationStatusId, s.RegistrationStatus1 })
                .ToList();

                statuses.Insert(0, new { RegistrationStatusId = (byte)'-', RegistrationStatus1 = "Все" });
                cmbStatus.ItemsSource = statuses;
                cmbStatus.SelectedIndex = 0;

                var distances = dbContext.EventType
                .Select(e => new { e.EventTypeId, e.EventTypeName }) // или e.EventName, если нет DistanceName
                .Distinct()
                .ToList();

                distances.Insert(0, new { EventTypeId = "", EventTypeName = "Все" });
                cmbDistance.ItemsSource = distances;
                cmbDistance.SelectedIndex = 0;

                cmbSortBy.ItemsSource = new[]
                {
                    new { DisplayName = "Имя", SortField = "FirstName" },
                    new { DisplayName = "Фамилия", SortField = "LastName" },
                    //new { DisplayName = "Email", SortField = "Email" }
                };
                cmbSortBy.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}");
            }
        }

        // Классы моделей (могут быть в отдельном файле)
        public class RunnerInfo
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public RegistrationStatus RegistrationStatus { get; set; }
        }

        public class RegistrationStatus
        {
            public int RegistrationStatusId { get; set; }
            public string RegistrationStatusName { get; set; }
        }

        public class Distance
        {
            public int DistanceId { get; set; }
            public string DistanceName { get; set; }
        }

        public class SortOption
        {
            public string DisplayName { get; set; }
            public string SortField { get; set; }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Остальные методы (кнопки и т.д.) остаются без изменений
        private void btnRefresh_Click(object sender, RoutedEventArgs e) { return; }//LoadRunners();
        private void btnBack_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string email)
            {
                // Находим бегуна по email
                var runnerInfo = (from r in dbContext.Registration
                                  join run in dbContext.Runner on r.RunnerId equals run.RunnerId
                                  join user in dbContext.User on run.Email equals user.Email
                                  join country in dbContext.Country on run.CountryCode equals country.CountryCode
                                  join charity in dbContext.Charity on r.CharityId equals charity.CharityId
                                  where user.Email == email
                                  select new RunnerInfoDto
                                  {
                                      Email = user.Email,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      Gender = run.Gender,
                                      DateOfBirth = run.DateOfBirth,
                                      Country = country.CountryName,
                                      Charity = charity.CharityName,
                                      SponsorshipTarget = r.SponsorshipTarget,
                                      RegistrationStatusId = r.RegistrationStatusId
                                  }).FirstOrDefault();

                if (runnerInfo != null)
                {
                    // Создаем и открываем страницу редактирования
                    var editPage = new RunnerDetails(runnerInfo);
                    NavigationService.Navigate(editPage);
                }
            }
        }

        private void btnExportCSV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Открытие диалога выбора файла
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "RunnersData",
                    DefaultExt = ".csv",
                    Filter = "CSV files (*.csv)|*.csv"
                };

                if (dlg.ShowDialog() == true)
                {
                    string filePath = dlg.FileName;
                    var runners = (from r in dbContext.Registration
                                   join runner in dbContext.Runner on r.RunnerId equals runner.RunnerId
                                   join user in dbContext.User on runner.Email equals user.Email
                                   join status in dbContext.RegistrationStatus on r.RegistrationStatusId equals status.RegistrationStatusId
                                   join country in dbContext.Country on runner.CountryCode equals country.CountryCode
                                   join regEvent in dbContext.RegistrationEvent on r.RegistrationId equals regEvent.RegistrationId
                                   join eventObj in dbContext.Event on regEvent.EventId equals eventObj.EventId
                                   join eventType in dbContext.EventType on eventObj.EventTypeId equals eventType.EventTypeId
                                   select new
                                   {
                                       user.FirstName,
                                       user.LastName,
                                       user.Email,
                                       runner.Gender,
                                       CountryName = country.CountryName,
                                       runner.DateOfBirth,
                                       r.RegistrationStatus.RegistrationStatus1,
                                       r.RaceKitOption,
                                       EventTypeName = eventType.EventTypeName
                                   }).ToList();

                    // Применение фильтров
                    if (byte.TryParse(cmbStatus.SelectedValue?.ToString(), out byte statusId) && statusId != (byte)'-')
                        runners = runners.Where(r => r.RegistrationStatus1 == dbContext.RegistrationStatus.FirstOrDefault(s => s.RegistrationStatusId == statusId).RegistrationStatus1).ToList();

                    string selectedDistanceId = cmbDistance.SelectedValue as string;
                    if (!string.IsNullOrEmpty(selectedDistanceId))
                        runners = runners.Where(r => r.EventTypeName == dbContext.EventType.FirstOrDefault(d => d.EventTypeId == selectedDistanceId).EventTypeName).ToList();

                    // Группировка по бегунам (на случай нескольких дистанций)
                    var grouped = runners
                        .GroupBy(r => r.Email)
                        .Select(g =>
                        {
                            var first = g.First();
                            var birthDateStr = first.DateOfBirth?.ToShortDateString() ?? "не указано";
                            var age = first.DateOfBirth.HasValue ? CalculateAge(first.DateOfBirth.Value, new DateTime(2015, 9, 5)) : 0;
                            var marathonTypes = string.Join(", ", g.Select(x => x.EventTypeName).Distinct());

                            return $"{first.FirstName},{first.LastName},{first.Email},{first.Gender},{first.CountryName},{birthDateStr},{age},{first.RegistrationStatus1},{first.RaceKitOption},{marathonTypes}";
                        }).ToList();

                    // Заголовки CSV
                    var lines = new List<string>
            {
                "Имя,Фамилия,Эл. адрес,Пол,Страна,Дата рождения,Возраст,Состояние регистрации,Комплект,Тип марафонов"
            };
                    lines.AddRange(grouped);

                    // Сохраняем файл
                    File.WriteAllLines(filePath, lines, System.Text.Encoding.UTF8);
                    MessageBox.Show("CSV-файл успешно сохранён.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте CSV: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для вычисления возраста
        private int CalculateAge(DateTime birthDate, DateTime referenceDate)
        {
            int age = referenceDate.Year - birthDate.Year;
            if (birthDate > referenceDate.AddYears(-age)) age--;
            return age;
        }


        private void btnExportEmails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var runners = (from r in dbContext.Registration
                               join runner in dbContext.Runner on r.RunnerId equals runner.RunnerId
                               join user in dbContext.User on runner.Email equals user.Email
                               join status in dbContext.RegistrationStatus on r.RegistrationStatusId equals status.RegistrationStatusId
                               join regEvent in dbContext.RegistrationEvent on r.RegistrationId equals regEvent.RegistrationId
                               join eventObj in dbContext.Event on regEvent.EventId equals eventObj.EventId
                               join eventType in dbContext.EventType on eventObj.EventTypeId equals eventType.EventTypeId
                               select new
                               {
                                   user.FirstName,
                                   user.LastName,
                                   user.Email,
                                   StatusId = r.RegistrationStatusId,
                                   EventTypeName = eventType.EventTypeName,
                                   EventTypeId = eventType.EventTypeId
                               }).ToList();

                // Применение фильтра по статусу
                if (byte.TryParse(cmbStatus.SelectedValue?.ToString(), out byte statusId) && statusId != (byte)'-')
                {
                    runners = runners.Where(r => r.StatusId == statusId).ToList();
                }

                // Применение фильтра по дистанции
                string selectedDistanceId = cmbDistance.SelectedValue as string;
                if (!string.IsNullOrEmpty(selectedDistanceId))
                {
                    runners = runners.Where(r => r.EventTypeId == selectedDistanceId).ToList();
                }

                // Группировка по Email, чтобы не было повторений
                var grouped = runners
                    .GroupBy(r => r.Email)
                    .Select(g =>
                    {
                        var first = g.First();
                        return $"\"{first.LastName} {first.FirstName}\" <{first.Email}>";
                    })
                    .ToList();

                string emailsFormatted = string.Join(";\n", grouped);

                // Показываем во всплывающем окне (можно копировать)
                var emailWindow = new Window
                {
                    Title = "Список адресов электронной почты",
                    Width = 600,
                    Height = 400,
                    Content = new TextBox
                    {
                        Text = emailsFormatted,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        AcceptsReturn = true,
                        AcceptsTab = true,
                        IsReadOnly = true,
                        Margin = new Thickness(10)
                    }
                };

                emailWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании списка email-ов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadRunners()
        {
            var query = from r in dbContext.Registration
                        join run in dbContext.Runner on r.RunnerId equals run.RunnerId
                        join status in dbContext.RegistrationStatus on r.RegistrationStatusId equals status.RegistrationStatusId
                        join user in dbContext.User on run.Email equals user.Email
                        join even in dbContext.RegistrationEvent on r.RegistrationId equals even.RegistrationId
                        select new
                        {
                            user.FirstName,
                            user.LastName,
                            user.Email,
                            Status = status.RegistrationStatus1,
                            r.RegistrationStatusId,
                            DistanceId = even.Event.EventType.EventTypeId
                        };

            // Фильтрация по статусу
            if (byte.TryParse(cmbStatus.SelectedValue.ToString(), out byte selectedStatusIdB) && selectedStatusIdB != (byte)'-')
            {
                query = query.Where(q => q.RegistrationStatusId == selectedStatusIdB);
            }

            string selectedDistanceId = cmbDistance.SelectedValue as string;
            if (!string.IsNullOrEmpty(selectedDistanceId))
            {
                query = query.Where(q => q.DistanceId == selectedDistanceId);
            }


            // Сортировка
            if (cmbSortBy.SelectedValue != null)
            {
                string sortField = cmbSortBy.SelectedValue.ToString();
                switch (sortField)
                {
                    case "FirstName":
                        query = query.OrderBy(q => q.LastName);
                        break;
                    case "LastName":
                        query = query.OrderBy(q => q.FirstName);
                        break;
                    case "Email":
                        query = query.OrderBy(q => q.Email);
                        break;
                }
            }

            dgRunners.ItemsSource = query.ToList();
            txtTotalRunners.Text = $"Всего бегунов: {query.Count()}";
        }

        private void cmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadRunners();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadRunners();
        }
    }
}

