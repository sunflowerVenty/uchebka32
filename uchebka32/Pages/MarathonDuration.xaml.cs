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
    /// Логика взаимодействия для MarathonDuration.xaml
    /// </summary>
    public partial class MarathonDuration : Page
    {
        public class SpeedItem
        {
            public string Name { get; set; }
            public double Speed { get; set; }
            public string ImageSource { get; set; }
            public string SpeedInfo => $"Скорость: {Speed} км/ч";
        }

        public class DistanceItem
        {
            public string Name { get; set; }
            public double Length { get; set; }
            public string ImageSource { get; set; }
            public string LengthInfo => $"Длина: {Length} м";
        }

        private List<SpeedItem> _speedItems;
        private List<DistanceItem> _distanceItems;
        public MarathonDuration()
        {
            InitializeComponent();
            LoadItems();
            ShowSpeedItems();
        }

        private void LoadItems()
        {
            _speedItems = new List<SpeedItem>
            {
                new SpeedItem { Name = "F1 Car", Speed = 345, ImageSource = "/Resources/f1-car.jpg" },
                new SpeedItem { Name = "Slug", Speed = 0.01, ImageSource = "/Resources/slug.jpg" },
                new SpeedItem { Name = "Horse", Speed = 15, ImageSource = "/Resources/horse.png" },
                new SpeedItem { Name = "Sloth", Speed = 0.12, ImageSource = "/Resources/sloth.jpg" },
                new SpeedItem { Name = "Capybara", Speed = 35, ImageSource = "/Resources/capybara.jpg" },
                new SpeedItem { Name = "Jaguar", Speed = 80, ImageSource = "/Resources/jaguar.jpg" },
                new SpeedItem { Name = "Worm", Speed = 0.03, ImageSource = "/Resources/worm.jpg" }
            };

            _distanceItems = new List<DistanceItem>
            {
                new DistanceItem { Name = "Bus", Length = 10, ImageSource = "/Resources/bus.jpg" },
                new DistanceItem { Name = "Pair of Havaianas", Length = 0.245, ImageSource = "/Resources/pair-of-havaianas.jpg" },
                new DistanceItem { Name = "Airbus A380", Length = 73, ImageSource = "/Resources/airbus-a380.jpg" },
                new DistanceItem { Name = "Football Field", Length = 105, ImageSource = "/Resources/football-field.jpg" },
                new DistanceItem { Name = "Ronaldinho", Length = 1.81, ImageSource = "/Resources/ronaldinho.jpg" }
            };

            listSpeed.ItemsSource = _speedItems;
            listDistance.ItemsSource = _distanceItems;
        }

        private void ShowSpeedItems()
        {
            listSpeed.Visibility = Visibility.Visible;
            listDistance.Visibility = Visibility.Collapsed;
            btnSpeed.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 144, 62));
            btnDistance.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(104, 159, 56));
        }

        private void ShowDistanceItems()
        {
            listSpeed.Visibility = Visibility.Collapsed;
            listDistance.Visibility = Visibility.Visible;
            btnDistance.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 144, 62));
            btnSpeed.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(104, 159, 56));
        }

        private void BtnSpeed_Click(object sender, RoutedEventArgs e)
        {
            ShowSpeedItems();
        }

        private void BtnDistance_Click(object sender, RoutedEventArgs e)
        {
            ShowDistanceItems();
        }

        private void ListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == listSpeed && listBox.SelectedItem is SpeedItem speedItem)
            {
                txtSelectedItem.Text = speedItem.Name;
                imgSelected.Source = new BitmapImage(new Uri(speedItem.ImageSource, UriKind.Relative));
                txtSelectedInfo.Text = $"Скорость: {speedItem.Speed} км/ч\n" +
                                      $"Время марафона: {CalculateMarathonTime(speedItem.Speed)}";
            }
            else if (listBox == listDistance && listBox.SelectedItem is DistanceItem distanceItem)
            {
                txtSelectedItem.Text = distanceItem.Name;
                imgSelected.Source = new BitmapImage(new Uri(distanceItem.ImageSource, UriKind.Relative));
                txtSelectedInfo.Text = $"Длина: {distanceItem.Length} м\n" +
                                      $"Марафон = {CalculateMarathonLength(distanceItem.Length)} таких объектов";
            }
        }

        private string CalculateMarathonTime(double speed)
        {
            if (speed <= 0) return "∞";
            double hours = 42.195 / speed;
            int h = (int)hours;
            int m = (int)((hours - h) * 60);
            return $"{h} ч {m} мин";
        }

        private string CalculateMarathonLength(double length)
        {
            if (length <= 0) return "∞";
            double count = 42195 / length;
            return count.ToString("N0");
        }

    }
}
