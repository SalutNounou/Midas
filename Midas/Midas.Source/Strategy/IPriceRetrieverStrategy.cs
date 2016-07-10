using System.Collections.Generic;
using Midas.Model;
using Midas.Model.Documents;

namespace Midas.Source.Strategy
{
    public interface IPriceRetrieverStrategy
    {
        IEnumerable<Security> RetrievePriceForSecurities(IEnumerable<Security> securities);
    }


    public interface IStatementRetrieverStrategy
    {
        IEnumerable<SecurityAndStatements> RetrieveStatementsForSecurities(IEnumerable<Security> securities);
    }

    public class SecurityAndStatements
    {
        public Security Security { get; set; }
        public List<FinancialStatement> FinancialStatements { get; set; }
    }
}