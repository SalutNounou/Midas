using System.Windows;
using System.Windows.Controls;
using Midas.ViewModels;

namespace Midas.Views
{
    /// <summary>
    /// Interaction logic for acquirersMultipleView.xaml
    /// </summary>
    public partial class AcquirersMultipleView : UserControl
    {
        public AcquirersMultipleView()
        {
            _viewModel = new AcquirersMultipleViewModel("Acquirer's Multiple");
            InitializeComponent();
            DataContext = _viewModel;
        }

        private readonly AcquirersMultipleViewModel _viewModel;

        private void GoButton_OnClick(object sender, RoutedEventArgs e)
        {
           _viewModel.RefreshSecurities();
        }

      
    }
}
