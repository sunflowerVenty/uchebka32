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
using System.Windows.Threading;
using uchebka32.Database;

namespace uchebka32.Pages
{
    /// <summary>
    /// Логика взаимодействия для InventoryArrival.xaml
    /// </summary>
    public partial class InventoryArrival : Page
    {
        MarafonUchebkaEntities _db;
        public InventoryArrival()
        {
            try
            {
                InitializeComponent();
                _db = new MarafonUchebkaEntities();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService?.GoBack();
            }
        }

        private void Kit_Checked(object sender, RoutedEventArgs e)
        {
            UpdateKitDescription();
        }

        private void UpdateKitDescription()
        {
            if (txtKitDescription == null) return;

            txtKitDescription.Text = rbKitA.IsChecked == true ? "Номер бегуна + RFID браслет" :
                                   rbKitB.IsChecked == true ? "Комплект A + Бейсболка + Бутылка воды" :
                                   "Комплект B + Футболка + Сувенирный буклет";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Введите корректное количество", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var kitType = rbKitA.IsChecked == true ? "A" :
                            rbKitB.IsChecked == true ? "B" : "C";

                // Используем System.Linq для FirstOrDefault
                var inventory = _db.Inventory.AsEnumerable().FirstOrDefault(i => i.RaceKitOptionId == kitType);

                if (inventory == null)
                {
                    _db.Inventory.Add(new Inventory
                    {
                        RaceKitOptionId = kitType,
                        Count = quantity
                    });
                }
                else
                {
                    inventory.Count += quantity;
                }

                _db.SaveChanges();
                MessageBox.Show($"Добавлено {quantity} комплектов типа {kitType}", "Успех");
                txtQuantity.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка");
            }
        }

        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
