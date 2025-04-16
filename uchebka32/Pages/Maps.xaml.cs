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
    /// Логика взаимодействия для Maps.xaml
    /// </summary>
    public partial class Maps : Page
    {
        public Maps()
        {
            InitializeComponent();
            InfoGrid.Visibility = Visibility.Hidden;
        }

        private void Check1_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Visible;
            Landmark.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Hidden;

            NameText.Text = "Checkpoint 1";
            LandmarkText.Text = "Avenida Rudge";

            PanelServises.Children.Clear();

            var itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-drinks.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock = new TextBlock
            {
                Text = "Drinks",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image2 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-energy-bars.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock2 = new TextBlock
            {
                Text = "Energy Bars",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            itemStackPanel.Children.Add(image);
            itemStackPanel.Children.Add(textBlock);

            itemStackPanel2.Children.Add(image2);
            itemStackPanel2.Children.Add(textBlock2);

            PanelServises.Children.Add(itemStackPanel);
            PanelServises.Children.Add(itemStackPanel2);
        }

        private void Start1_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Hidden;
            Landmark.Visibility = Visibility.Hidden;
            PanelServises.Children.Clear();

            NameText.Text = "Race Start";
            StartText.Text = "Samba Full Marathon";
            LandmarkText.Text = "";
        }

        private void Check7_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Visible;
            Landmark.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Hidden;

            NameText.Text = "Checkpoint 7";
            LandmarkText.Text = "Cemitério da Consolação";

            PanelServises.Children.Clear();

            var itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-drinks.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock = new TextBlock
            {
                Text = "Drinks",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image2 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-energy-bars.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock2 = new TextBlock
            {
                Text = "Energy Bars",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image3 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-toilets.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock3 = new TextBlock
            {
                Text = "Toilets",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel4 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image4 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-information.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock4 = new TextBlock
            {
                Text = "Information",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel5 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image5 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-medical.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock5 = new TextBlock
            {
                Text = "Medical",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            itemStackPanel.Children.Add(image);
            itemStackPanel.Children.Add(textBlock);

            itemStackPanel2.Children.Add(image2);
            itemStackPanel2.Children.Add(textBlock2);

            itemStackPanel3.Children.Add(image3);
            itemStackPanel3.Children.Add(textBlock3);

            itemStackPanel4.Children.Add(image4);
            itemStackPanel4.Children.Add(textBlock4);

            itemStackPanel5.Children.Add(image5);
            itemStackPanel5.Children.Add(textBlock5);

            PanelServises.Children.Add(itemStackPanel);
            PanelServises.Children.Add(itemStackPanel2);
            PanelServises.Children.Add(itemStackPanel3);
            PanelServises.Children.Add(itemStackPanel4);
            PanelServises.Children.Add(itemStackPanel5);
        }

        private void Check5_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Visible;
            Landmark.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Hidden;

            NameText.Text = "Checkpoint 5";
            LandmarkText.Text = "Iguatemi";


            PanelServises.Children.Clear();

            var itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-drinks.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock = new TextBlock
            {
                Text = "Drinks",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image2 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-energy-bars.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock2 = new TextBlock
            {
                Text = "Energy Bars",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image3 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-toilets.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock3 = new TextBlock
            {
                Text = "Toilets",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel4 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image4 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-information.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock4 = new TextBlock
            {
                Text = "Information",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            itemStackPanel.Children.Add(image);
            itemStackPanel.Children.Add(textBlock);

            itemStackPanel2.Children.Add(image2);
            itemStackPanel2.Children.Add(textBlock2);

            itemStackPanel3.Children.Add(image3);
            itemStackPanel3.Children.Add(textBlock3);

            itemStackPanel4.Children.Add(image4);
            itemStackPanel4.Children.Add(textBlock4);

            PanelServises.Children.Add(itemStackPanel);
            PanelServises.Children.Add(itemStackPanel2);
            PanelServises.Children.Add(itemStackPanel3);
            PanelServises.Children.Add(itemStackPanel4);
        }

        private void Start2_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Hidden;
            Landmark.Visibility = Visibility.Hidden;
            PanelServises.Children.Clear();

            NameText.Text = "Race Start";
            StartText.Text = "Jongo Half Marathon";
            LandmarkText.Text = "";
        }

        private void Check4_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Visible;
            Landmark.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Hidden;

            NameText.Text = "Checkpoint 4";
            LandmarkText.Text = "Jardim Luzitania";

            PanelServises.Children.Clear();

            var itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-drinks.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock = new TextBlock
            {
                Text = "Drinks",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image2 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-energy-bars.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock2 = new TextBlock
            {
                Text = "Energy Bars",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image3 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-toilets.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock3 = new TextBlock
            {
                Text = "Toilets",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel5 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image5 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-medical.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock5 = new TextBlock
            {
                Text = "Medical",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            itemStackPanel.Children.Add(image);
            itemStackPanel.Children.Add(textBlock);

            itemStackPanel2.Children.Add(image2);
            itemStackPanel2.Children.Add(textBlock2);

            itemStackPanel3.Children.Add(image3);
            itemStackPanel3.Children.Add(textBlock3);

            itemStackPanel5.Children.Add(image5);
            itemStackPanel5.Children.Add(textBlock5);

            PanelServises.Children.Add(itemStackPanel);
            PanelServises.Children.Add(itemStackPanel2);
            PanelServises.Children.Add(itemStackPanel3);
            PanelServises.Children.Add(itemStackPanel5);
        }

        private void Check3_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Visible;
            Landmark.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Hidden;

            NameText.Text = "Checkpoint 3";
            LandmarkText.Text = "Parque do Ibirapuera";

            PanelServises.Children.Clear();

            var itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-drinks.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock = new TextBlock
            {
                Text = "Drinks",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image2 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-energy-bars.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock2 = new TextBlock
            {
                Text = "Energy Bars",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image3 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-toilets.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock3 = new TextBlock
            {
                Text = "Toilets",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            itemStackPanel.Children.Add(image);
            itemStackPanel.Children.Add(textBlock);

            itemStackPanel2.Children.Add(image2);
            itemStackPanel2.Children.Add(textBlock2);

            itemStackPanel3.Children.Add(image3);
            itemStackPanel3.Children.Add(textBlock3);

            PanelServises.Children.Add(itemStackPanel);
            PanelServises.Children.Add(itemStackPanel2);
            PanelServises.Children.Add(itemStackPanel3);
        }

        private void Check2_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Visible;
            Landmark.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Hidden;

            NameText.Text = "Checkpoint 2";
            LandmarkText.Text = "Theatro Municipal";

            PanelServises.Children.Clear();

            var itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-drinks.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock = new TextBlock
            {
                Text = "Drinks",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image2 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-energy-bars.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock2 = new TextBlock
            {
                Text = "Energy Bars",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image3 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-toilets.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock3 = new TextBlock
            {
                Text = "Toilets",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel4 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image4 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-information.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock4 = new TextBlock
            {
                Text = "Information",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel5 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image5 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-medical.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock5 = new TextBlock
            {
                Text = "Medical",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            itemStackPanel.Children.Add(image);
            itemStackPanel.Children.Add(textBlock);

            itemStackPanel2.Children.Add(image2);
            itemStackPanel2.Children.Add(textBlock2);

            itemStackPanel3.Children.Add(image3);
            itemStackPanel3.Children.Add(textBlock3);

            itemStackPanel4.Children.Add(image4);
            itemStackPanel4.Children.Add(textBlock4);

            itemStackPanel5.Children.Add(image5);
            itemStackPanel5.Children.Add(textBlock5);

            PanelServises.Children.Add(itemStackPanel);
            PanelServises.Children.Add(itemStackPanel2);
            PanelServises.Children.Add(itemStackPanel3);
            PanelServises.Children.Add(itemStackPanel4);
            PanelServises.Children.Add(itemStackPanel5);
        }

        private void Check8_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Visible;
            Landmark.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Hidden;

            NameText.Text = "Checkpoint 8";
            LandmarkText.Text = "Cemitério da Consolação";

            PanelServises.Children.Clear();

            var itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-drinks.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock = new TextBlock
            {
                Text = "Drinks",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image2 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-energy-bars.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock2 = new TextBlock
            {
                Text = "Energy Bars",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image3 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-toilets.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock3 = new TextBlock
            {
                Text = "Toilets",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel4 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image4 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-information.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock4 = new TextBlock
            {
                Text = "Information",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel5 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image5 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-medical.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock5 = new TextBlock
            {
                Text = "Medical",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            itemStackPanel.Children.Add(image);
            itemStackPanel.Children.Add(textBlock);

            itemStackPanel2.Children.Add(image2);
            itemStackPanel2.Children.Add(textBlock2);

            itemStackPanel3.Children.Add(image3);
            itemStackPanel3.Children.Add(textBlock3);

            itemStackPanel4.Children.Add(image4);
            itemStackPanel4.Children.Add(textBlock4);

            itemStackPanel5.Children.Add(image5);
            itemStackPanel5.Children.Add(textBlock5);

            PanelServises.Children.Add(itemStackPanel);
            PanelServises.Children.Add(itemStackPanel2);
            PanelServises.Children.Add(itemStackPanel3);
            PanelServises.Children.Add(itemStackPanel4);
            PanelServises.Children.Add(itemStackPanel5);
        }

        private void Start3_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Hidden;
            Landmark.Visibility = Visibility.Hidden;
            PanelServises.Children.Clear();

            NameText.Text = "Race Start";
            StartText.Text = "Capoeira 5km Fun Run";
            LandmarkText.Text = "";
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Hidden;
            Landmark.Visibility = Visibility.Hidden;
            PanelServises.Children.Clear();

            NameText.Text = "Finish";
            StartText.Text = "";
            LandmarkText.Text = "";
        }

        private void Check6_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Visible;
            ServicesPr.Visibility = Visibility.Visible;
            Landmark.Visibility = Visibility.Visible;
            StartText.Visibility = Visibility.Hidden;

            NameText.Text = "Checkpoint 6";
            LandmarkText.Text = "Rua Lisboa";

            PanelServises.Children.Clear();

            var itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-drinks.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock = new TextBlock
            {
                Text = "Drinks",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel2 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image2 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-energy-bars.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock2 = new TextBlock
            {
                Text = "Energy Bars",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            var itemStackPanel3 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var image3 = new Image
            {
                Source = new BitmapImage(new Uri("C:/Users/202214/Source/Repos/uchebka32/uchebka32/Images/Maps/map-icons/map-icon-toilets.png")),
                Width = 45,
                Margin = new Thickness(5)
            };
            var textBlock3 = new TextBlock
            {
                Text = "Toilets",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 25
            };

            itemStackPanel.Children.Add(image);
            itemStackPanel.Children.Add(textBlock);

            itemStackPanel2.Children.Add(image2);
            itemStackPanel2.Children.Add(textBlock2);

            itemStackPanel3.Children.Add(image3);
            itemStackPanel3.Children.Add(textBlock3);

            PanelServises.Children.Add(itemStackPanel);
            PanelServises.Children.Add(itemStackPanel2);
            PanelServises.Children.Add(itemStackPanel3);
        }

        private void KrestBtn_Click(object sender, RoutedEventArgs e)
        {
            InfoGrid.Visibility = Visibility.Hidden;
        }
    }
}
