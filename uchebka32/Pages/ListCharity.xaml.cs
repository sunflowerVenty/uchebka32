using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using uchebka32.Database;

namespace uchebka32.Pages
{
    public class Organization
    {
        private string _logoPath;

        public BitmapImage Logo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Organization(string logoPath, string name, string description)
        {
            _logoPath = logoPath;
            Logo = new BitmapImage(new Uri($"C:/Users/202214/source/repos/uchebka32/Images/Charity/{_logoPath}", UriKind.Absolute));
            Name = name;
            Description = description;
        }
    }
    public partial class ListCharity : Page
    {
        public ObservableCollection<Organization> Organizations { get; set; }

        public ListCharity()
        {
            InitializeComponent();
            DataContext = this;

            Organizations = new ObservableCollection<Organization>();
            LoadOrganizationsFromDatabase();
        }

        private void LoadOrganizationsFromDatabase()
        {
            var dbOrganizations = ConnnectionDB.buEntities.Charity.ToList();

            foreach (var dbOrg in dbOrganizations)
            {
                Organizations.Add(new Organization(
                    dbOrg.CharityLogo,
                    dbOrg.CharityName,
                    dbOrg.CharityDescription
                ));
            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Console.WriteLine($"Ошибка загрузки изображения: {e.ErrorException.Message}");
        }
    }
}