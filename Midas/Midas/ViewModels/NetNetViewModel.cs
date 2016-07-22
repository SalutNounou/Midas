using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using Midas.DAL.SecuritiesDal;
using Midas.Model;
using Midas.Model.BlackList;
using Midas.Engines.Engines;
using Midas.Engines.Strategy;
using Midas.Model.DataSources;
using Midas.Model.MarketData;

namespace Midas.ViewModels
{
    public class NetNetViewModel:ItemViewModel
    {

        private  ISourceEngine _priceEngine;
        private  ISourceEngine _statementsEngine;
        private  ISourceEngine _ncavEngine;

        public NetNetViewModel(string tabName) : base(tabName)
        {
            RefreshNetNets();
        }

        public void RefreshNetNets()
        {
            NetNetSecurities.Clear();
            SecurityDalFactory.GetInstance()
                .GetSecurityDal()
                .GetAllSecurities()
                .Where(x => x.DiscountOnNcav > DiscountOnNcavThresholdPercent/100)
                .Where(x => !BlackList.IsBlackListed(x.Ticker))
                .OrderBy(x => x.DiscountOnNcav).Reverse()
                .ToList()
                .ForEach(NetNetSecurities.Add);
        }

        private decimal _discountOntNcavThresholdPercent =25;

        public decimal DiscountOnNcavThresholdPercent
        {
            get
            {
                return _discountOntNcavThresholdPercent;
            }
            set
            {
                if (_discountOntNcavThresholdPercent == value) return;
                _discountOntNcavThresholdPercent = value;
                OnPropertyChanged("DiscountOnNcavThresholdPercent");
                RefreshNetNets();
            }
        }

        public void RecalculateDiscountsOnNcav()
        {
            _priceEngine = new PriceEngine(new SecurityPriceRetrieverStrategy(new YahooMarketDataPriceSource()));
            while (_priceEngine.ShouldWork)
            {
                _priceEngine.DoCycle();
            }
            _ncavEngine = new NcavEngine();
            while (_ncavEngine.ShouldWork)
            {
                _ncavEngine.DoCycle();
            }
            RefreshNetNets();
        }

        public void ImportFinancialsStatements()
        {
            string apiKey = ConfigurationManager.AppSettings["EdgarApiKey"];
            _statementsEngine = new StatementEngine(new StatementRetrieverStrategy(new EdgarMarketDataFinancialStatementSource(apiKey)));
            while (_statementsEngine.ShouldWork)
            {
                _statementsEngine.DoCycle();
            }
            RefreshNetNets();
        }


        public ObservableCollection<Security> NetNetSecurities = new ObservableCollection<Security>();



    }
}