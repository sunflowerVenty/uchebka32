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
using System.Windows.Threading;
using System.Data.Entity;
using uchebka32.Database;

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для MyRaceResults.xaml
    /// </summary>
    public partial class MyRaceResults : Page
    {
        private string currentUserEmail;
        private Runner currentRunner;
        public MyRaceResults(string email)
        {
            InitializeComponent();
            currentUserEmail = email;

            Loaded += MyRaceResultsPage_Loaded;
        }

        private void MyRaceResultsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRunnerDataAndResults();
        }
        private void LoadRunnerDataAndResults()
        {
            using (var context = new MarafonUchebkaEntities())
            {
                currentRunner = context.Runner
                    .Include(r => r.User)
                    .FirstOrDefault(r => r.Email == currentUserEmail);

                if (currentRunner == null)
                {
                    MessageBox.Show("Не удалось загрузить данные пользователя.");
                    return;
                }

                // Вывод пола и возрастной категории
                string genderText = currentRunner.Gender == "Male" ? "мужской" : "женский";
                GenderTextBlock.Text = $"Пол: {genderText}";

                int age = CalculateAge(currentRunner.DateOfBirth ?? DateTime.Now);
                AgeCategoryTextBlock.Text = $"Возрастная категория: {GetAgeCategory(age)}";

                // Загрузка результатов
                LoadResults(context, currentRunner.RunnerId, currentRunner.Gender, age);
            }
        }
        private void LoadResults(MarafonUchebkaEntities context, int runnerId, string gender, int age)
        {
            var category = GetAgeCategory(age);

            var results = (from re in context.RegistrationEvent
                           join reg in context.Registration on re.RegistrationId equals reg.RegistrationId
                           join ev in context.Event on re.EventId equals ev.EventId
                           join et in context.EventType on ev.EventTypeId equals et.EventTypeId
                           join m in context.Marathon on ev.MarathonId equals m.MarathonId
                           where reg.RunnerId == runnerId && re.RaceTime != null
                           select new
                           {
                               Marathon = m.MarathonName,
                               Distance = et.EventTypeName,
                               Time = re.RaceTime,
                               EventId = ev.EventId
                           }).ToList();

            var resultViewModels = new List<ResultViewModel>();

            foreach (var result in results)
            {
                TimeSpan raceTime = TimeSpan.FromSeconds((double)result.Time);
                string timeFormatted = raceTime.ToString(@"hh\:mm\:ss");

                var allTimes = context.RegistrationEvent
                    .Where(r => r.EventId == result.EventId && r.RaceTime != null)
                    .OrderBy(r => r.RaceTime)
                    .Select(r => r.RaceTime)
                    .ToList();

                int overallPlace = allTimes.IndexOf(result.Time) + 1;


                var categoryTimes = (from re in context.RegistrationEvent
                                     join reg in context.Registration on re.RegistrationId equals reg.RegistrationId
                                     join run in context.Runner on reg.RunnerId equals run.RunnerId
                                     where re.EventId == result.EventId &&
                                           re.RaceTime != null &&
                                           run.Gender == gender
                                     select new
                                     {
                                         RaceTime = re.RaceTime,
                                         BirthDate = run.DateOfBirth
                                     })
                                    .ToList()
                                    .Where(x => GetAgeCategory(CalculateAge(x.BirthDate ?? DateTime.Now)) == category)
                                    .OrderBy(x => x.RaceTime)
                                    .Select(x => x.RaceTime)
                                    .ToList();
                int categoryPlace = categoryTimes.IndexOf(result.Time) + 1;

                resultViewModels.Add(new ResultViewModel
                {
                    Marathon = result.Marathon,
                    Distance = result.Distance,
                    Time = timeFormatted,
                    OverallRank = overallPlace,
                    CategoryRank = categoryPlace
                });
            }

            if (resultViewModels.Count == 0)
            {
                MessageBox.Show("Вы пока не участвовали в соревнованиях.");
            }

            ResultsDataGrid.ItemsSource = resultViewModels;
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        private string GetAgeCategory(int age)
        {
            if (age < 18) return "до 18";
            if (age <= 29) return "18-29";
            if (age <= 39) return "30-39";
            if (age <= 55) return "40-55";
            if (age <= 70) return "56-70";
            return "более 70";
        }
        public class ResultViewModel
        {
            public string Marathon { get; set; }
            public string Distance { get; set; }
            public string Time { get; set; }
            public int OverallRank { get; set; }
            public int CategoryRank { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuRunner());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
