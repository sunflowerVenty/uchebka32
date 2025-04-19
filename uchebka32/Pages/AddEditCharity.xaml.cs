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
            CurrentLogoImage.Source = new BitmapImage(new Uri("/Images/Charity/" + charity.CharityLogo, UriKind.RelativeOrAbsolute));
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

                    try
                    {
                        // 1. Проверяем существование исходного файла
                        if (!File.Exists(openFileDialog.FileName))
                        {
                            MessageBox.Show("Исходный файл не найден!");
                            return;
                        }

                        // 2. Создаем целевую папку, если её нет
                        if (!Directory.Exists(targetFolder))
                        {
                            Directory.CreateDirectory(targetFolder);
                        }

                        // 3. Генерируем уникальное имя файла (на случай, если файл уже существует)
                        string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                        string destPath = System.IO.Path.Combine(targetFolder, fileName);

                        // 4. Если файл уже существует, добавляем к имени временную метку
                        if (File.Exists(destPath))
                        {
                            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            string fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
                            string fileExt = System.IO.Path.GetExtension(fileName);
                            destPath = System.IO.Path.Combine(targetFolder, $"{fileNameWithoutExt}_{timestamp}{fileExt}");
                        }
                        LogoPathTextBox.Text = fileName;

                        // 5. Копируем файл
                        File.Copy(openFileDialog.FileName, destPath, overwrite: true);

                        if (!string.IsNullOrEmpty(LogoPathTextBox.Text))
                        {
                            CurrentLogoImage.Source = new BitmapImage(new Uri(destPath, UriKind.RelativeOrAbsolute));
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Ошибка: Нет прав для записи в целевую папку");
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Ошибка ввода-вывода: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Неожиданная ошибка: {ex.Message}");
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

            ConnnectionDB.buEntities.SaveChanges();

            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
