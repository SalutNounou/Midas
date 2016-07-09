using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Midas.Model.MarketData
{
    public interface IMarketDataPriceSource
    {
        Task<Fixings> GetFixingAsync(string ticker);
        Task<IEnumerable<HistoricalFixing>> GetHistoricalFixingsAsync(string ticker, DateTime dateFrom, DateTime dateTo);
        Task<int> GetNbOutstandingSharesAsync(string ticker);
        Task<IEnumerable<LastAndOutstanding>> GetLastAndOutstandingAsync(IEnumerable<string> tickers);
        IEnumerable<LastAndOutstanding> GetLastAndOutstanding(IEnumerable<string> tickers);
    }
}