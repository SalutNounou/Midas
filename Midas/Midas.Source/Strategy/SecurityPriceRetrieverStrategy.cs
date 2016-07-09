using System;
using System.Collections.Generic;
using System.Linq;
using Midas.Model;
using Midas.Model.MarketData;

namespace Midas.Source.Strategy
{
    public class SecurityPriceRetrieverStrategy : IPriceRetrieverStrategy
    {
        private readonly IMarketDataPriceSource _priceSource;

        public SecurityPriceRetrieverStrategy(IMarketDataPriceSource priceSource)
        {
            this._priceSource = priceSource;
        }


        public IEnumerable<Security> RetrievePriceForSecurities(IEnumerable<Security> securities)
        {
            var securitiesList = securities as IList<Security> ?? securities.ToList();
            var listLastAndOutstanding = _priceSource.GetLastAndOutstanding(securitiesList.Select(s => s.Ticker));
            foreach (var lastAndOutstanding in listLastAndOutstanding)
            {
                var security = securitiesList.FirstOrDefault(s => s.Ticker == lastAndOutstanding.Ticker);
                if (security != null)
                {
                    security.Last = lastAndOutstanding.Last;
                    security.NbSharesOutstanding = lastAndOutstanding.SharesOutstanding;
                    security.MarketCapitalisation = lastAndOutstanding.MarketCapitalisation;
                    if (security.Last > 0)
                        security.DateOfLatestPrice = DateTime.Today;
                }
            }
            return securitiesList;
        }
    }
}