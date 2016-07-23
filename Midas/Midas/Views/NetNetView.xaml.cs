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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Midas.ViewModels;

namespace Midas.Views
{
    /// <summary>
    /// Interaction logic for NetNetView.xaml
    /// </summary>
    public partial class NetNetView : UserControl
    {
        public NetNetView()
        {
            InitializeComponent();
            _viewModel = new NetNetViewModel("Net Nets");
            this.DataContext = _viewModel;
            NetNetGrid.ItemsSource = _viewModel.NetNetSecurities;
        }


        private readonly NetNetViewModel _viewModel;

        private void DiscountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(_viewModel!=null)
                _viewModel.DiscountOnNcavThresholdPercent = (decimal)DiscountSlider.Value;
        }

        private async void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = await ((MetroWindow) Window.GetWindow(this)).ShowMessageAsync("Financial Statements Import", "This operation can take some time. Do you still wish to proceed?",MessageDialogStyle.AffirmativeAndNegative);
            if (result != MessageDialogResult.Affirmative) return;
            _viewModel.ImportFinancialsStatements();
            await
                ((MetroWindow) Window.GetWindow(this)).ShowMessageAsync("Financial Statements Import",
                    "Import Successful!");
        }

        private async void CalculateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = await ((MetroWindow)Window.GetWindow(this)).ShowMessageAsync("Calculation of discount on Net Current Asset Value", "This operation can take some time. Do you still wish to proceed?", MessageDialogStyle.AffirmativeAndNegative);
            if (result != MessageDialogResult.Affirmative) return;
            _viewModel.RecalculateDiscountsOnNcav();
            await
                ((MetroWindow)Window.GetWindow(this)).ShowMessageAsync("Calculation of discount on Net Current Asset Value",
                    "Calculation Finished.");
        }
    }
}
