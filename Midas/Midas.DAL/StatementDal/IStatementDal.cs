using System.Collections.Generic;
using Midas.Model;
using Midas.Model.Documents;

namespace Midas.DAL.StatementDal
{
    public interface IStatementDal
    {
        bool ImportStatements(IEnumerable<FinancialStatement> statements);
        IEnumerable<FinancialStatement> GetAllStatements();
    }


    public interface IStatementDalFactory
    {
        IStatementDal GetSecurityDal();
    }


    public class StatementDalFactory : IStatementDalFactory
    {

        private static StatementDalFactory _instance;

        private StatementDalFactory()
        {

        }

        private static readonly object LockObj = new object();
        public static StatementDalFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (LockObj)
                {
                    _instance = new StatementDalFactory();
                }
            }
            return _instance;
        }

        public IStatementDal GetSecurityDal()
        {
            return new StatementDalEf();
        }
    }
}