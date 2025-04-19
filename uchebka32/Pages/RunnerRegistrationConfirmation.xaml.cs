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
    /// Логика взаимодействия для RunnerRegistrationConfirmation.xaml
    /// </summary>
    public partial class RunnerRegistrationConfirmation : Page
    {
        public RunnerRegistrationConfirmation()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MenuRunner());
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayoutForSize(e.NewSize.Width);
        }

        private void UpdateLayoutForSize(double width)
        {
            if (width < 600) // Мобильные устройства
            {
                SetFontSize("HeaderTextStyle", 20);
                SetFontSize("TitleTextStyle", 18);
                SetFontSize("NormalTextStyle", 14);
                SetFontSize("ItalicTextStyle", 14);
                SetFontSize("CountdownTextStyle", 12);
            }
            else if (width < 800) // Планшеты
            {
                SetFontSize("HeaderTextStyle", 24);
                SetFontSize("TitleTextStyle", 20);
                SetFontSize("NormalTextStyle", 16);
                SetFontSize("ItalicTextStyle", 16);
                SetFontSize("CountdownTextStyle", 14);
            }
            else // Десктопы
            {
                SetFontSize("HeaderTextStyle", 28);
                SetFontSize("TitleTextStyle", 24);
                SetFontSize("NormalTextStyle", 18);
                SetFontSize("ItalicTextStyle", 18);
                SetFontSize("CountdownTextStyle", 16);
            }
        }

        private void SetFontSize(string styleKey, double size)
        {
            if (Resources[styleKey] is Style style)
            {
                style.Setters.Add(new Setter(TextBlock.FontSizeProperty, size));
            }
        }
    }
}
