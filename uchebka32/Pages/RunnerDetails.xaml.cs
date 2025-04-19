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

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для RunnerDetails.xaml
    /// </summary>
    public partial class RunnerDetails : Page
    {
        public RunnerDetails(RunnerInfoDto runnerEmail)
        {
            InitializeComponent();
            LoadRunnerData(runnerEmail);
        }

        private void LoadRunnerData(RunnerInfoDto runnerInfo)
        {
            txtEmail.Text = runnerInfo.Email;
            txtFirstName.Text = runnerInfo.FirstName;
            txtLastName.Text = runnerInfo.LastName;
            txtGender.Text = runnerInfo.GenderDisplay;
            txtBirthDate.Text = runnerInfo.BirthDateDisplay;
            txtCountry.Text = runnerInfo.Country;
            txtCharity.Text = runnerInfo.Charity;
            txtDonation.Text = runnerInfo.DonationDisplay;

            UpdateRegistrationStatus(runnerInfo.RegistrationStatusId);

        }

        private void UpdateRegistrationStatus(int statusId)
        {
            // Сбросить все статусы
            ellipseRegistered.Fill = Brushes.Red;
            ellipsePaid.Fill = Brushes.Red;
            ellipseKit.Fill = Brushes.Red;
            ellipseStarted.Fill = Brushes.Red;

            // Установить актуальные статусы
            switch (statusId)
            {
                case 1: // Зарегистрирован
                    ellipseRegistered.Fill = Brushes.Green;
                    break;
                case 2: // Подтверждена оплата
                    ellipseRegistered.Fill = Brushes.Green;
                    ellipsePaid.Fill = Brushes.Green;
                    break;
                case 3: // Выдан пакет
                    ellipseRegistered.Fill = Brushes.Green;
                    ellipsePaid.Fill = Brushes.Green;
                    ellipseKit.Fill = Brushes.Green;
                    break;
                case 4: // Вышел на старт
                    ellipseRegistered.Fill = Brushes.Green;
                    ellipsePaid.Fill = Brushes.Green;
                    ellipseKit.Fill = Brushes.Green;
                    ellipseStarted.Fill = Brushes.Green;
                    break;
            }
        }

        private void btnCertificate_Click(object sender, RoutedEventArgs e)
        {
            // Логика показа сертификата
        }

        private void btnBadge_Click(object sender, RoutedEventArgs e)
        {
            // Логика печати бейджа
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            //
        }

    }
}
