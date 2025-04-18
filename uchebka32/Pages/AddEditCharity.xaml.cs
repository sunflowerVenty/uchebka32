using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddEditCharity.xaml
    /// </summary>
    public partial class AddEditCharity : Page
    {
        Charity charity;
        public AddEditCharity(Charity _charity)
        {
            charity = _charity;
            InitializeComponent();

            LogoPathTextBox.Text = charity.CharityLogo;
            NameTextBox.Text = charity.CharityName;
            DescriptionTextBox.Text = charity.CharityDescription;
        }

        private void BrowseLogo_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string projectFolder = AppDomain.CurrentDomain.BaseDirectory;
                    string targetFolder = System.IO.Path.Combine(projectFolder, "Images", "Charity");

                    string sourcePath = openFileDialog.FileName;
                    string fileName = System.IO.Path.GetFileName(sourcePath);
                    string destPath = System.IO.Path.Combine(targetFolder, fileName);
                    File.Copy(sourcePath, destPath, overwrite: true);
                    
                    LogoPathTextBox.Text = fileName;

                    if (!string.IsNullOrEmpty(LogoPathTextBox.Text))
                    {
                        CurrentLogoImage.Source = new BitmapImage(new Uri("/Images/Charity/" + LogoPathTextBox.Text, UriKind.RelativeOrAbsolute));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, укажите наименование организации");
                return;
            }
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, укажите описание организации");
                return;
            }

            charity.CharityName = NameTextBox.Text;
            charity.CharityDescription = DescriptionTextBox.Text;
            charity.CharityLogo = LogoPathTextBox.Text;
            ConnnectionDB.buEntities.Charity.Add(charity);
            ConnnectionDB.buEntities.SaveChanges();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
