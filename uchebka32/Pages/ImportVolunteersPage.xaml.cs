using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using uchebka32.Database;

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для ImportVolunteersPage.xaml
    /// </summary>
    public partial class ImportVolunteersPage : Page
    {
        private readonly BegunUchebkaEntities _context;
        private int _totalImported = 0;
        private int _totalUpdated = 0;
        public ImportVolunteersPage()
        {
            InitializeComponent();
            _context = ConnnectionDB.buEntities;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Пожалуйста, выберите CSV файл.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                ImportButton_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var log = new StringBuilder();
            _totalImported = 0;
            _totalUpdated = 0;

            try
            {
                var lines = File.ReadAllLines(txtFilePath.Text);
                if (lines.Length < 2)
                {
                    throw new Exception("CSV файл пуст или не содержит данных.");
                }

                // Проверяем заголовки
                var headers = lines[0].Split(',');
                if (headers.Length != 5 ||
                    !headers.Contains("VolunteerId") ||
                    !headers.Contains("FirstName") ||
                    !headers.Contains("LastName") ||
                    !headers.Contains("CountryCode") ||
                    !headers.Contains("Gender"))
                {
                    throw new Exception("Неверный формат CSV файла. Проверьте заголовки колонок.");
                }

                using (var context = new BegunUchebkaEntities())
                {
                    // Обрабатываем каждую строку
                    for (int i = 1; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(',');
                        if (values.Length != 5)
                        {
                            log.AppendLine($"Ошибка в строке {i + 1}: неверное количество полей");
                            continue;
                        }

                        try
                        {
                            var volunteerId = int.Parse(values[0].Trim());
                            var firstName = values[1].Trim();
                            var lastName = values[2].Trim();
                            var countryCode = values[3].Trim();
                            var gender = values[4].Trim();

                            // Проверяем пол
                            if (gender != "Female" && gender != "Male")
                            {
                                log.AppendLine($"Ошибка в строке {i + 1}: неверный пол ({gender})");
                                continue;
                            }

                            // Получаем страну
                            var country = context.Country.FirstOrDefault(c => c.CountryCode == countryCode);
                            if (country == null)
                            {
                                log.AppendLine($"Предупреждение: Страна с кодом {countryCode} не найдена");
                                continue;
                            }

                            // Проверяем существование волонтера
                            var existingVolunteer = context.Volunteer.FirstOrDefault(v => v.VolunteerId == volunteerId);
                            if (existingVolunteer != null)
                            {
                                // Обновляем существующего волонтера
                                existingVolunteer.FirstName = firstName;
                                existingVolunteer.LastName = lastName;
                                existingVolunteer.CountryCode = countryCode;
                                existingVolunteer.Gender = gender;
                                _totalUpdated++;
                                log.AppendLine($"Обновлен волонтер: {firstName} {lastName} (ID: {volunteerId})");
                            }
                            else
                            {
                                // Создаем нового волонтера
                                var volunteer = new Volunteer
                                {
                                    VolunteerId = volunteerId,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    CountryCode = countryCode,
                                    Gender = gender
                                };
                                context.Volunteer.Add(volunteer);
                                _totalImported++;
                                log.AppendLine($"Добавлен новый волонтер: {firstName} {lastName} (ID: {volunteerId})");
                            }
                        }
                        catch (FormatException)
                        {
                            log.AppendLine($"Ошибка в строке {i + 1}: неверный формат ID волонтера");
                        }
                        catch (Exception ex)
                        {
                            log.AppendLine($"Ошибка в строке {i + 1}: {ex.Message}");
                        }
                    }

                    context.SaveChanges();
                }

                // Добавляем итоговую статистику
                log.AppendLine();
                log.AppendLine($"Итого:");
                log.AppendLine($"Добавлено новых записей: {_totalImported}");
                log.AppendLine($"Обновлено записей: {_totalUpdated}");

                txtImportLog.Text = log.ToString();

                if (_totalImported > 0 || _totalUpdated > 0)
                {
                    MessageBox.Show($"Импорт завершен успешно.\nДобавлено: {_totalImported}\nОбновлено: {_totalUpdated}",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                log.AppendLine($"Критическая ошибка: {ex.Message}");
                if (ex.InnerException != null)
                {
                    log.AppendLine($"Детали: {ex.InnerException.Message}");
                }
                txtImportLog.Text = log.ToString();
                MessageBox.Show("Произошла ошибка при импорте. Проверьте лог для деталей.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

    }
}
