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
using System.Windows.Shapes;
using uchebka32.Pages;
using uchebka32.Database;

namespace uchebka32.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow(List<InventoryManagement.ReportItem> reportData)
        {
            InitializeComponent();
            LoadReportData();
            dgReport.ItemsSource = reportData;
        }

        private void LoadReportData()
        {
            try
            {
                var db = ConnnectionDB.buEntities;
                var reportData = new List<InventoryManagement.ReportItem>();

                // Получаем данные для отчета
                var inventory = db.Inventory.ToList();
                var registrations = db.Registration.ToList();
                var kitOptions = db.RaceKitOption.ToList();

                // Пример заполнения данных (замените на вашу логику)
                foreach (var option in kitOptions)
                {
                    var stockItem = inventory.FirstOrDefault(i => i.RaceKitOptionId == option.RaceKitOptionId);
                    int registeredCount = registrations.Count(r => r.RaceKitOptionId == option.RaceKitOptionId);

                    reportData.Add(new InventoryManagement.ReportItem
                    {
                        Name = option.RaceKitOption1,
                        Stock = stockItem?.Count ?? 0,
                        Required = registeredCount
                    });
                }

                // Добавляем дополнительные элементы из вашего примера
                reportData.Add(new InventoryManagement.ReportItem { Name = "Номер бегуна", Stock = 500, Required = 1000 });
                reportData.Add(new InventoryManagement.ReportItem { Name = "RFID браслет", Stock = 800, Required = 1000 });
                reportData.Add(new InventoryManagement.ReportItem { Name = "Бейсболка", Stock = 1200, Required = 1000 });
                reportData.Add(new InventoryManagement.ReportItem { Name = "Бутылка воды", Stock = 2000, Required = 1000 });
                reportData.Add(new InventoryManagement.ReportItem { Name = "Футболка", Stock = 900, Required = 1000 });
                reportData.Add(new InventoryManagement.ReportItem { Name = "Сувенирный буклет", Stock = 1500, Required = 1000 });

                dgReport.ItemsSource = reportData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных отчета: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Здесь можно добавить логику для печати
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(dgReport, "Отчет по инвентарю");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при печати: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
