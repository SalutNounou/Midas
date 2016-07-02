using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Midas.Model.MarketData
{
    public interface IMarketDataPriceSource
    {
        Task<Fixings> GetFixing(string ticker);
        Task<IEnumerable<HistoricalFixing>> GetHistoricalFixings(string ticker, DateTime dateFrom, DateTime dateTo);
        Task<int> GetNbOutstandingShares(string ticker);
    }
}