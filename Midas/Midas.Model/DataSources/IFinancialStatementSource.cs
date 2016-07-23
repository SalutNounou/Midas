using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midas.Model.Documents;
using System.Threading.Tasks;


namespace Midas.Model.DataSources
{
    public interface IFinancialStatementSource
    {
        Task<ObservableCollection<FinancialStatement>> GetAnnualFinancialStatementsAsync(string ticker);
        Task<ObservableCollection<FinancialStatement>> GetQuarterlyFinancialStatementsAsync(string ticker);
        ObservableCollection<FinancialStatement> GetAnnualFinancialStatements(string ticker);
        ObservableCollection<FinancialStatement> GetAnnualFinancialStatements(IEnumerable<string> tickers);
        ObservableCollection<FinancialStatement> GetQuarterlyFinancialStatements(string ticker);
        ObservableCollection<FinancialStatement> GetQuarterlyFinancialStatements(IEnumerable<string> tickers);
    }
}


