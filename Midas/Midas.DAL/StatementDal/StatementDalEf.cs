using System.Collections.Generic;
using Midas.Model.Documents;

namespace Midas.DAL.StatementDal
{
    public class StatementDalEf : IStatementDal
    {
        public bool ImportStatements(IEnumerable<FinancialStatement> statements)
        {
            return false;
        }

        public IEnumerable<FinancialStatement> GetAllStatements()
        {
            using (var unitOfWork = new UnitOfWork(new MidasContext()))
            {
                return unitOfWork.FinancialStatements.GetAll();
            }
        }
    }
}