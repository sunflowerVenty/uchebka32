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
    /// Логика взаимодействия для CheckRunner.xaml
    /// </summary>
    public partial class CheckRunner : Page
    {
        public Frame Frame1;
        public CheckRunner(Frame frame)
        {
            Frame1 = frame;
            InitializeComponent();
        }

        private void OldRunnerBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage(Frame1));
        }

        private void NewRunnerBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegRunner());
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            NavigationService.Navigate(new MenuRunner());
        }
    }
}
