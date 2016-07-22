using System.Data.Entity;
using Midas.DAL;
using log4net.Config;

namespace Midas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MidasContext>());
            InitializeComponent();
            BasicConfigurator.Configure();
        }

        

      
    }
}
