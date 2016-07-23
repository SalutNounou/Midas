using System.Collections.ObjectModel;

namespace Midas.ViewModels
{
    public class MainWindowViewModel
    {
        private ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();

        public ObservableCollection<ItemViewModel> Items => items;

        public MainWindowViewModel()
        {
            Items.Add(new NetNetViewModel(tabName:"Net Net"));
            Items.Add(new ItemViewModel(tabName: "Acquirer's Multiple"));
            Items.Add(new ImportViewModel(tabName: "Import Securities"));
        }
    }
}