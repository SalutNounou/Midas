using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using log4net;

namespace Midas.Model.MarketData
{


    public class YahooMarketDataPriceSource : IMarketDataPriceSource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(YahooMarketDataPriceSource));

        public async Task<IEnumerable<LastAndOutstanding>> GetLastAndOutstandingAsync(IEnumerable<string> tickers)
        {
            try
            {
                string csvData;

                using (WebClient web = new WebClient())
                {
                    csvData = await web.DownloadStringTaskAsync(string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f=sl1j2j1", String.Join("+", tickers)));
                }

                IEnumerable<LastAndOutstanding> data = ParseLastAndOutstanding(csvData);
                return data;

            }
            catch (Exception)
            {
                return new List<LastAndOutstanding>();

            }
        }

        public IEnumerable<LastAndOutstanding> GetLastAndOutstanding(IEnumerable<string> tickers)
        {
            try
            {
                string csvData;

                using (WebClient web = new WebClient())
                {
                    csvData =  web.DownloadString(string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f=sl1j2j1", String.Join("+", tickers)));
                }

                IEnumerable<LastAndOutstanding> data = ParseLastAndOutstanding(csvData);
                return data;

            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0}{1}", exc.Message, exc.StackTrace));
                return new List<LastAndOutstanding>();

            }
        }




        public async Task<Fixings> GetFixingAsync(string ticker)
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
            catch (Exception)
            {
                return new Fixings();
            }


        }

        public async Task<int> GetNbOutstandingSharesAsync(string ticker)
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
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<HistoricalFixing>> GetHistoricalFixingsAsync(string ticker, DateTime dateFrom, DateTime dateTo)
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
            catch (Exception)
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

        public static IEnumerable<LastAndOutstanding> ParseLastAndOutstanding(string toParse)
        {
            var data = new List<LastAndOutstanding>();
            string[] rows = toParse.Replace("\r", "").Split('\n');
            foreach (string row in rows)
            {
                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(',');

                LastAndOutstanding lastAndOutstanding = new LastAndOutstanding();
                lastAndOutstanding.Ticker = cols[0].Replace("\"", "");
                if (cols[1] != "N/A")
                    lastAndOutstanding.Last = Convert.ToDecimal(cols[1], CultureInfo.InvariantCulture);
                if (cols[2] != "N/A")
                    lastAndOutstanding.SharesOutstanding = Convert.ToDecimal(cols[2], CultureInfo.InvariantCulture);
                if (cols[3] != "N/A")
                    lastAndOutstanding.MarketCapitalisation = HandleMarketCap(cols[3]);
                data.Add(lastAndOutstanding);
            }
            return data;
        }

        public static Decimal HandleMarketCap(string toParse)
        {
            try
            {
                char lastChar = toParse.ElementAt(toParse.Length - 1);
                Decimal multiple = 1;
                switch (lastChar)
                {
                    case 'B':
                        multiple = 1000000000;
                        break;
                    case 'M':
                        multiple = 1000000;
                        break;
                }
                toParse = toParse.Remove(toParse.Length - 1);
                return Convert.ToDecimal(toParse, CultureInfo.InvariantCulture)*multiple;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0} - {1}", exc.Message, exc.StackTrace));
            }
            return 0;
        }

    }
}