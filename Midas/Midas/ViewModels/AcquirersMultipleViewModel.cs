using System;
using System.Collections.ObjectModel;
using System.Linq;
using Midas.DAL.SecuritiesDal;
using Midas.Model;
using Midas.Model.BlackList;

namespace Midas.ViewModels
{
    public class AcquirersMultipleViewModel : ItemViewModel
    {
        public AcquirersMultipleViewModel(string tabName) : base(tabName)
        {
            AcquirersMultipleSecurityViewModels = new ObservableCollection<AcquirersMultipleSecurityViewModel>();
            RefreshSecurities();
        }


        public void RefreshSecurities()
        {
            AcquirersMultipleSecurityViewModels.Clear();
            var allSecurities =
                SecurityDalFactory.GetInstance()
                    .GetSecurityDal()
                    .GetAllSecurities()
                    .Where(x => x.AcquirersMultiple > 0)
                    .Where(x => x.IsMarketCapBigEnough(MarketCapitalizationMin))
                    .Where(x => !x.IsMarketCapTooBig(MarketCapitalizationMax))
                    .Where(x => !BlackList.IsBlackListed(x.Ticker))
                    .Where(x => x.OperatingEarnings > 0)
                    .OrderBy(x => x.AcquirersMultiple)
                    .ToList();
            var currentRank = 0;
            foreach (var security in allSecurities)
            {
                currentRank++;
                AcquirersMultipleSecurityViewModels.Add(new AcquirersMultipleSecurityViewModel(security, currentRank));
            }

        }

        private Int64 _marketCapitalizationMin = 10000000;//10 millions by default

        public Int64 MarketCapitalizationMin
        {
            get
            {
                return _marketCapitalizationMin;
            }
            set
            {
                if (_marketCapitalizationMin == value) return;
                _marketCapitalizationMin = value;
                OnPropertyChanged("MarketCapitalizationMin");
                RefreshSecurities();
            }
        }

        private Int64 _marketCapMax = 100000000;//100 millions by default
        public Int64 MarketCapitalizationMax
        {
            get
            {
                return _marketCapMax;
            }
            set
            {
                if (_marketCapMax == value) return;
                _marketCapMax = value;
                OnPropertyChanged("MarketCapitalizationMax");
                RefreshSecurities();
            }
        }



        public ObservableCollection<AcquirersMultipleSecurityViewModel> AcquirersMultipleSecurityViewModels { get; }




    }


    public class AcquirersMultipleSecurityViewModel
    {
        private readonly Model.Security _security;

        public AcquirersMultipleSecurityViewModel(Security security, int rank)
        {
            _security = security;
            Rank = rank;
        }

        public int Rank { get; }

        public string Ticker => _security.Ticker;

        public string Name => _security.Name;

        public Decimal AcquirersMultiple => _security.AcquirersMultiple;

        public Decimal EnterpriseValue => _security.EnterpriseValue;

        public Decimal OperatingEarnings => _security.OperatingEarnings;

        public Decimal Last => _security.Last;

        public Decimal MarketCap
        {
            get
            {
                if (_security.NbSharesOutstanding > 0)
                    return _security.Last * _security.NbSharesOutstanding;
                return _security.MarketCapitalisation;
            }
        }
    }


}