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
using uchebka32.Windows;

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для InventoryManagement.xaml
    /// </summary>
    public partial class InventoryManagement : Page
    {
        public class ReportItem
        {
            public string Name { get; set; }
            public int Stock { get; set; }
            public int Required { get; set; }
            public int ToOrder => Math.Max(0, Required - Stock);
        }

        public class InventoryItem
        {
            public string RaceKitOption { get; set; }
            public int TotalInStock { get; set; }
            public int RegisteredCount { get; set; }
            public int Remaining => TotalInStock - RegisteredCount;
        }
        public InventoryManagement()
        {
            InitializeComponent();
            LoadInventoryData();
        }

        private void LoadInventoryData()
        {
            try
            {
                var db = ConnnectionDB.buEntities;
                var inventoryData = new List<InventoryItem>();

                // Получаем общее количество зарегистрированных бегунов
                int totalRunners = db.Registration.Count();
                txtTotalRunners.Text = $"Всего зарегистрировано бегунов: {totalRunners}";

                // Получаем данные по комплектам
                var kitOptions = db.RaceKitOption.ToList();
                var inventory = db.Inventory.ToList();
                var registrations = db.Registration.ToList();

                foreach (var option in kitOptions)
                {
                    var stockItem = inventory.FirstOrDefault(i => i.RaceKitOptionId == option.RaceKitOptionId);
                    int registeredCount = registrations.Count(r => r.RaceKitOptionId == option.RaceKitOptionId);

                    inventoryData.Add(new InventoryItem
                    {
                        RaceKitOption = option.RaceKitOption1,
                        TotalInStock = stockItem?.Count ?? 0,
                        RegisteredCount = registeredCount
                    });
                }

                dgInventory.ItemsSource = inventoryData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            LoadInventoryData();
            MessageBox.Show("Данные успешно обновлены", "Обновление",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = ConnnectionDB.buEntities;

                // Получаем данные из базы
                var registrations = db.Registration.ToList();
                var inventoryItems = db.Inventory.ToList();
                var kitOptions = db.RaceKitOption.ToList();

                int totalRunners = registrations.Count;

                // Считаем сколько зарегистрировано каждого типа комплекта
                int countA = registrations.Count(r => r.RaceKitOptionId == "A");
                int countB = registrations.Count(r => r.RaceKitOptionId == "B");
                int countC = registrations.Count(r => r.RaceKitOptionId == "C");

                // Получаем общее количество каждого комплекта на складе
                int totalA = inventoryItems.FirstOrDefault(i => i.RaceKitOptionId == "A")?.Count ?? 0;
                int totalB = inventoryItems.FirstOrDefault(i => i.RaceKitOptionId == "B")?.Count ?? 0;
                int totalC = inventoryItems.FirstOrDefault(i => i.RaceKitOptionId == "C")?.Count ?? 0;

                // Рассчитываем реальные остатки (сколько есть минус сколько используется)
                int remainingA = totalA - countA;
                int remainingB = totalB - countB;
                int remainingC = totalC - countC;

                var reportItems = new List<ReportItem>();

                // 1. Номер бегуна (есть во всех комплектах)
                reportItems.Add(new ReportItem
                {
                    Name = "Номер бегуна",
                    Stock = remainingA + remainingB + remainingC,
                    Required = totalRunners
                });

                // 2. RFID браслет (только в комплекте A)
                reportItems.Add(new ReportItem
                {
                    Name = "RFID браслет",
                    Stock = remainingA,
                    Required = countA
                });

                // 3. Бейсболка (в комплектах B и C)
                reportItems.Add(new ReportItem
                {
                    Name = "Бейсболка",
                    Stock = remainingB + remainingC,
                    Required = countB + countC
                });

                // 4. Бутылка воды (в комплектах B и C)
                reportItems.Add(new ReportItem
                {
                    Name = "Бутылка воды",
                    Stock = remainingB + remainingC,
                    Required = countB + countC
                });

                // 5. Футболка (только в комплекте C)
                reportItems.Add(new ReportItem
                {
                    Name = "Футболка",
                    Stock = remainingC,
                    Required = countC
                });

                // 6. Сувенирный буклет (только в комплекте C)
                reportItems.Add(new ReportItem
                {
                    Name = "Сувенирный буклет",
                    Stock = remainingC,
                    Required = countC
                });

                var reportWindow = new ReportWindow(reportItems);
                reportWindow.Owner = Window.GetWindow(this);
                reportWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnArrival_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InventoryArrival());
        }
    }
}
