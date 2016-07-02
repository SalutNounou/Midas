using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Midas.Model.SecuritiesImport
{
    public interface ISecurityImporter
    {
        Task<IEnumerable<Security>> ImportSecuritiesAsync(string path);
    }


    public interface ISecurityImporterFactory
    {
        ISecurityImporter GetSecurityImporter(string marketName);
    }

    public class SecurityImporterFactory : ISecurityImporterFactory
    {
        public ISecurityImporter GetSecurityImporter(string marketName)
        {
            switch (marketName)
            {
                case Constants.MarketConstants.Nyse:
                case Constants.MarketConstants.Amex:
                case Constants.MarketConstants.Nasdaq:
                    return new NyseSecurityImporter(marketName);
                default:
                    throw new Exception(String.Format("The importer for market {0} is not implemented.", marketName));
            }
            
        }
    }
}