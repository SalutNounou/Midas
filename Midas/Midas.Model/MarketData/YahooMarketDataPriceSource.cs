using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Midas.Model.MarketData
{
    public class YahooMarketDataPriceSource : IMarketDataPriceSource
    {
        public async Task<Fixings> GetFixing(string ticker)
        {
            try
            {
                string csvData;

                using (WebClient web = new WebClient())
                {
                    csvData = await web.DownloadStringTaskAsync(string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f=snbaopl1",
                        ticker));
                }

                var fixings = ParseFixings(csvData);
                return fixings.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Fixings();
            }


        }

        public async Task<int> GetNbOutstandingShares(string ticker)
        {
            try
            {
                string csvData;

                using (WebClient web = new WebClient())
                {
                    csvData = await web.DownloadStringTaskAsync(string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f=j2", ticker));
                }

                return Convert.ToInt32(csvData);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<HistoricalFixing>> GetHistoricalFixings(string ticker, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string csvData;

                using (WebClient web = new WebClient())
                {
                    csvData = await web.DownloadStringTaskAsync(string.Format("http://ichart.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g=d&ignore=.csv", ticker, dateFrom.Month - 1, dateFrom.Day, dateFrom.Year, dateTo.Month - 1, dateTo.Day, dateTo.Year));
                }

                return ParseHistoricalFixings(csvData);

            }
            catch (Exception ex)
            {
                return new List<HistoricalFixing>();
            }
        }


        private IEnumerable<Fixings> ParseFixings(string toParse)
        {
            List<Fixings> prices = new List<Fixings>();

            string[] rows = toParse.Replace("\r", "").Split('\n');

            foreach (string row in rows)
            {
                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(',');

                Fixings p = new Fixings();
                p.Symbol = cols[0];
                p.Name = cols[1];
                p.Bid = Convert.ToDecimal(cols[2], CultureInfo.InvariantCulture);
                p.Ask = Convert.ToDecimal(cols[3], CultureInfo.InvariantCulture);
                p.Open = Convert.ToDecimal(cols[4], CultureInfo.InvariantCulture);
                p.PreviousClose = Convert.ToDecimal(cols[5], CultureInfo.InvariantCulture);
                p.Last = Convert.ToDecimal(cols[6], CultureInfo.InvariantCulture);

                prices.Add(p);
            }

            return prices;
        }
        private IEnumerable<HistoricalFixing> ParseHistoricalFixings(string toParse)
        {
            var prices = new List<HistoricalFixing>();
            var rows = toParse.Replace("\r", "").Split('\n').ToList();
            rows.RemoveAt(0);

            foreach (string row in rows)
            {
                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(',');

                HistoricalFixing p = new HistoricalFixing();
                p.Date = Convert.ToDateTime(cols[0]);
                p.Open = Convert.ToDecimal(cols[1], CultureInfo.InvariantCulture);
                p.High = Convert.ToDecimal(cols[2], CultureInfo.InvariantCulture);
                p.Low = Convert.ToDecimal(cols[3], CultureInfo.InvariantCulture);
                p.Close = Convert.ToDecimal(cols[4], CultureInfo.InvariantCulture);
                p.Volume = Convert.ToDecimal(cols[5], CultureInfo.InvariantCulture);
                p.AdjClose = Convert.ToDecimal(cols[6], CultureInfo.InvariantCulture);

                prices.Add(p);
            }

            return prices;
        }



    }
}