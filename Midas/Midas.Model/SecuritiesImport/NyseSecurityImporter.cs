using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Midas.Model.SecuritiesImport
{
    public class NyseSecurityImporter : AbstractSecurityImporter
    {

        public NyseSecurityImporter(string marketName):base(marketName)
        {
        }

        public override async Task<IEnumerable<Security>> ImportSecuritiesAsync(string path)
        {
            var securities = new List<Security>();
            try
            {
                string text = await ReadTextAsync(path);
                var lines = text.Split('\n').ToList();
                //We don't care for the first line : "Symbol","Name","LastSale","MarketCap","IPOyear","Sector","industry","Summary Quote"
                lines.RemoveAt(0);
                foreach (string line in lines)
                {
                    var columns = line.Split(',');
                    if (columns.Count() > 2)
                    {
                        Security security = new Security
                        {
                            Ticker = Convert.ToString(columns[0],CultureInfo.InvariantCulture).Replace("\"","").Trim(),
                            Name = Convert.ToString(columns[1],CultureInfo.InvariantCulture).Replace("\"","").Trim(),
                            Currency = "USD",
                            Market = _marketName
                        };
                        securities.Add(security);
                    }
                }
                return securities;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}