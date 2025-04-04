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
    /// Логика взаимодействия для ListCharity.xaml
    /// </summary>
    public partial class ListCharity : Page
    {
        public ListCharity()
        {
            InitializeComponent();
        }

        private void AddUserToStackPanel(string logo, string name, string desc)
        {
            StackPanel charityPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 10, 0, 10)
            };

            Image image = new Image
            {
                Source = new BitmapImage(new Uri("/Images/Source/" + logo, UriKind.Absolute)),
                Width = 50,
                Margin = new Thickness(10, 0, 10, 0)
            };

            StackPanel NameDescPanel = new StackPanel
            {

            };

            TextBlock nameBlock = new TextBlock
            {
                Text = $"{name}",
                FontSize = 30,
                Margin = new Thickness(0, 0, 5, 0)
            };

            TextBlock descBlock = new TextBlock
            {
                Text = $"{desc}"
            };

            charityPanel.Children.Add(image);
            NameDescPanel.Children.Add(nameBlock);
            NameDescPanel.Children.Add(descBlock);
            charityPanel.Children.Add(NameDescPanel);

            ListPanel.Children.Add(charityPanel);
        }
    }
}
