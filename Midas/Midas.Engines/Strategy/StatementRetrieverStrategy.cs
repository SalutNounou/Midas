using System.Collections.Generic;
using System.Linq;
using Midas.Model;
using Midas.Model.DataSources;
using Midas.Model.Documents;

namespace Midas.Engines.Strategy
{
    public class StatementRetrieverStrategy : IStatementRetrieverStrategy
    {

        private readonly IFinancialStatementSource _financialStatementSource;

        public StatementRetrieverStrategy(IFinancialStatementSource financialStatementSource)
        {
            _financialStatementSource = financialStatementSource;
        }

        public IEnumerable<SecurityAndStatements> RetrieveStatementsForSecurities(IEnumerable<Security> securities)
        {
            var securitiesToUpdate = securities as IList<Security> ?? securities.ToList();

            var result = new List<SecurityAndStatements>();

            var statementsGrouped = _financialStatementSource.GetAnnualFinancialStatements(securitiesToUpdate.Select(s=>s.Ticker)).ToList().GroupBy(s => s.PrimarySymbol).ToDictionary(g=>g.Key, g=>g.ToList()); /*GetQuarterlyFinancialStatements*/


            foreach (var security in securitiesToUpdate)
            {
                SecurityAndStatements securityAndStatements = new SecurityAndStatements { Security = security };
                var statements = new List<FinancialStatement>();
                if (statementsGrouped.ContainsKey(security.Ticker))
                    statements = statementsGrouped[security.Ticker];
                if (statements.Any(f => f.FormType == "10-Q" || f.FormType == "10-K" || f.FormType=="20-F" || f.FormType =="40-F"))
                {

                    security.Has10Q = statements.Any(s => s.FormType == "10-Q");
                    security.Has10K = statements.Any(s => s.FormType == "10-K");
                    security.Has20F = statements.Any(s => s.FormType == "20-F");
                    security.Has40F = statements.Any(s => s.FormType == "40-F");
                    if (security.Has10Q)
                        security.DateOfLatest10QFinancialStatement = statements.Where(s => s.FormType == "10-Q").Max(x => x.ReceivedDate);
                    if (security.Has10K)
                        security.DateOfLatest10KFinancialStatement = statements.Where(s => s.FormType == "10-K").Max(x => x.ReceivedDate);
                    if (security.Has20F)
                        security.DateOfLatest20FFinancialStatement = statements.Where(s => s.FormType == "20-F").Max(x => x.ReceivedDate);
                    if (security.Has40F)
                        security.DateOfLatest40FFinancialStatement = statements.Where(s => s.FormType == "40-F").Max(x => x.ReceivedDate);
                    
                    securityAndStatements.FinancialStatements = statements.ToList();

                    result.Add(securityAndStatements);
                    continue;
                }


                securityAndStatements.FinancialStatements = new List<FinancialStatement>();
                security.NbOfFailedAttemptsToGetStatements++;
                result.Add(securityAndStatements);
            }
            return result;
        }

       
    }
}