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
    /// Логика взаимодействия для Volonteers.xaml
    /// </summary>
    public partial class Volonteers : Page
    {
        private readonly MarafonUchebkaEntities _context;
        private List<Volunteer> volunteers = new List<Volunteer>();
        public Volonteers()
        {
            InitializeComponent();
            _context = ConnnectionDB.buEntities;
            SortComboBox.SelectedIndex = 0;
            LoadVolunteers();
        }

        private void LoadVolunteers()
        {
            try
            {
                ApplySort();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке волонтеров: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplySort()
        {
            var selectedItem = SortComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            var query = _context.Volunteer.AsQueryable();

            switch (selectedItem.Content.ToString())
            {
                case "Фамилии":
                    query = query.OrderBy(v => v.LastName);
                    break;
                case "Имени":
                    query = query.OrderBy(v => v.FirstName);
                    break;
                case "Стране":
                    query = query.OrderBy(v => v.Country.CountryName);
                    break;
                case "Полу":
                    query = query.OrderBy(v => v.Gender);
                    break;
            }

            var volunteers = query
                .Select(v => new
                {
                    v.VolunteerId,
                    v.FirstName,
                    v.LastName,
                    CountryName = v.Country.CountryName ?? "Неизвестно",
                    Gender = v.Gender == "Male" ? "Мужской" : "Женский"
                })
                .ToList();

            VolunteersDataGrid.ItemsSource = volunteers;
            VolunteerCountText.Text = $"Всего волонтеров: {volunteers.Count}";
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadVolunteers();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySort();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ImportVolunteersPage());
        }
    }
}
