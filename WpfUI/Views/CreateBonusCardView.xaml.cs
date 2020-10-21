using BonusCardManager.WpfUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BonusCardManager.WpfUI.Views
{
    /// <summary>
    /// Interaction logic for CreateBonusCardView.xaml
    /// </summary>
    public partial class CreateBonusCardView : UserControl
    {
        public CreateBonusCardView()
        {
            InitializeComponent();
        }

        private void calendarLoaded(object sender, RoutedEventArgs e)
        {
            DatePicker cal = sender as DatePicker;
            cal.BlackoutDates.AddDatesInPast();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var model = this.DataContext as CreateBonusCardViewModel;
            if (model != null)
            {
                model.InitCustomers();
            }
        }
    }
}
