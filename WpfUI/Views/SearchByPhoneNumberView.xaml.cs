﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BonusCardManager.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for SearchByPhoneNumberView.xaml
    /// </summary>
    public partial class SearchByPhoneNumberView : UserControl
    {
        public SearchByPhoneNumberView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = (regex.IsMatch(e.Text));
        }
    }
}
