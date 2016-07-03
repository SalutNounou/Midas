using System.Collections.ObjectModel;

namespace Midas.ViewModels
{
    public class MainWindowViewModel
    {
        private ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();

        public ObservableCollection<ItemViewModel> Items => items;

        public MainWindowViewModel()
        {
            Items.Add(new ItemViewModel(tabName:"Net Net"));
            Items.Add(new ItemViewModel(tabName: "Greenblatt's Formula"));
            Items.Add(new ImportViewModel(tabName: "Import Securities"));
        }
    }
}