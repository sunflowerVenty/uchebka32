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
    /// Логика взаимодействия для PreviosPaseResults.xaml
    /// </summary>
    public partial class PreviosPaseResults : Page
    {
        public PreviosPaseResults()
        {
            InitializeComponent();
        }

        private int GetAge(DateTime? birthdate, DateTime marathonDate)
        {
            if (birthdate == null) return 0;
            int age = marathonDate.Year - birthdate.Value.Year;
            if (marathonDate < birthdate.Value.AddYears(age)) age--;
            return age;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadRunners();
        }

        public void LoadRunners()
        {
            using (var db = new MarafonUchebkaEntities())
            {
                var query = from r in db.Registration
                            join runner in db.Runner on r.RunnerId equals runner.RunnerId
                            join user in db.User on runner.Email equals user.Email
                            join regev in db.RegistrationEvent on r.RegistrationId equals regev.RegistrationId
                            join evt in db.Event on regev.EventId equals evt.EventId
                            join m in db.Marathon on evt.MarathonId equals m.MarathonId
                            join c in db.Country on runner.CountryCode equals c.CountryCode
                            select new
                            {
                                runner.Gender,
                                Birthdate = runner.DateOfBirth,
                                Time = regev.RaceTime,
                                user.FirstName,
                                user.LastName,
                                Country = c.CountryName,
                                MarathonId = m.MarathonId,
                                EventTypeId = evt.EventTypeId,
                                MarathonName = m.MarathonName + " " + m.YearHeld
                            };

                // Apply filters
                if (cmbMarathon.SelectedValue != null && int.TryParse(cmbMarathon.SelectedValue.ToString(), out int selectedMarathonId))
                {
                    query = query.Where(x => x.MarathonId == selectedMarathonId);
                }

                if (cmbDistance.SelectedValue != null)
                {
                    string selectedDistance = cmbDistance.SelectedValue.ToString();
                    query = query.Where(x => x.EventTypeId == selectedDistance);
                }

                if (cmbGender.SelectedItem != null)
                {
                    string gender = cmbGender.SelectedItem.ToString();
                    query = query.Where(x => x.Gender == gender);
                }

                if (cmbCategory.SelectedItem != null)
                {
                    DateTime now = DateTime.Now; // Или дата марафона из базы данных
                    string category = cmbCategory.SelectedItem.ToString();

                    // Фильтрация по возрастным категориям
                    switch (category)
                    {
                        case "до 18":
                            query = query.Where(x => x.Birthdate != null &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) < 18));
                            break;

                        case "18-29":
                            query = query.Where(x => x.Birthdate != null &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) >= 18 &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) <= 29)));
                            break;

                        case "30-39":
                            query = query.Where(x => x.Birthdate != null &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) >= 30 &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) <= 39)));
                            break;

                        case "40-55":
                            query = query.Where(x => x.Birthdate != null &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) >= 40 &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) <= 55)));
                            break;

                        case "56-70":
                            query = query.Where(x => x.Birthdate != null &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) >= 56 &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) <= 70)));
                            break;

                        case "более 70":
                            query = query.Where(x => x.Birthdate != null &&
                                (now.Year - x.Birthdate.Value.Year -
                                ((now.Month < x.Birthdate.Value.Month ||
                                 (now.Month == x.Birthdate.Value.Month && now.Day < x.Birthdate.Value.Day) ? 1 : 0)) > 70));
                            break;
                    }
                }

                var filteredResults = query
                    .Where(x => x.Time.HasValue)
                    .OrderBy(x => x.Time.Value)
                    .ToList();

                var result = new List<dynamic>();
                int position = 1;

                foreach (var item in filteredResults)
                {
                    result.Add(new
                    {
                        Position = position++,
                        Time = TimeSpan.FromSeconds(item.Time.Value).ToString(@"hh\:mm\:ss"),
                        RunnerName = $"{item.FirstName} {item.LastName}",
                        item.Country
                    });
                }

                dgRunners.ItemsSource = result;

                // Update statistics
                txtTotalRunners.Text = $"Всего бегунов: {query.Count()}";
                txtFinished.Text = $"Всего бегунов финишировало: {query.Count(x => x.Time != null)}";

                if (query.Any(x => x.Time != null))
                {
                    var avgTime = filteredResults.Average(x => x.Time.Value);
                    txtAvgTime.Text = $"Среднее время: {FormatTimeWithLabels(avgTime)}";
                }
                else
                {
                    txtAvgTime.Text = "Среднее время: N/A";
                }

                if (!result.Any())
                {
                    MessageBox.Show("Бегуны не найдены по заданным критериям.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private string FormatTimeWithLabels(double totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return $"{time.Hours:D2} ч {time.Minutes:D2} м {time.Seconds:D2} с";
        }

    }
}
