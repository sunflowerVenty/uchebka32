using System;
using System.Collections.Generic;
using System.Windows;
using uchebka32.Pages;

namespace uchebka32.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartFrame.NavigationService.Navigate(new MainPage(MainFrame));
        }
    }
}
