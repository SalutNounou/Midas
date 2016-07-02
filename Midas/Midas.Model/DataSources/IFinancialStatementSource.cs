using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midas.Model.Documents;
using System.Threading.Tasks;
using Midas.Model.DataSources;

namespace Midas.Model.DataSources
{
    public interface IFinancialStatementSource
    {
        Task<ObservableCollection<FinancialStatement>> GetAnnualFinancialStatementsAsync(string ticker);
        Task<ObservableCollection<FinancialStatement>> GetQuarterlyFinancialStatementsAsync(string ticker);
    }
}


