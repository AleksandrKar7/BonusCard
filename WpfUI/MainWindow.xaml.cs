﻿using BonusCardManager.WpfUI.ViewModels;
using System.Windows;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new NavigationViewModel();
        }
    }
}
