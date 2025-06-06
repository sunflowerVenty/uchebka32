﻿using System;
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
    /// Логика взаимодействия для MenuKoor.xaml
    /// </summary>
    public partial class MenuKoor : Page
    {
        public MenuKoor()
        {
            InitializeComponent();
        }

        private void RunnersButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RunnersManagement());
        }

        private void SponsorsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SponsorsOverview());
        }

    }
}
