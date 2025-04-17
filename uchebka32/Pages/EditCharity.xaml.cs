using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using uchebka32.Database;

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditCharity.xaml
    /// </summary>
    public partial class EditCharity : Page
    {
        public EditCharity()
        {
            InitializeComponent();
            LoadCompaniesData();
        }

        private void LoadCompaniesData()
        {
            var companies = ConnnectionDB.buEntities.Charity
                .Select(c => new
                {
                    c.CharityId,
                    c.CharityName,
                    c.CharityDescription,
                    LogoPath = "/Images/Charity/" + c.CharityLogo
                })
                .ToList();

            CompaniesDataGrid.ItemsSource = companies;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Charity selectedCharity)
            {
                NavigationService.Navigate(new AddEditCharity(selectedCharity));
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Charity newCharity = new Charity();
            newCharity.CharityName = "";
            newCharity.CharityDescription = "";
            newCharity.CharityLogo = "";
            ConnnectionDB.buEntities.Charity.Add(newCharity);
            ConnnectionDB.buEntities.SaveChanges();

            NavigationService.Navigate(new AddEditCharity(newCharity));
        }
    }
}

