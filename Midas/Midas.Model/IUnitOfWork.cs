using Midas.Model.Documents;


namespace Midas.Model
{
    public interface IUnitOfWork
    {
        IRepository<Security> Securities { get; }
        IRepository<FinancialStatement> FinancialStatements { get; } 
        int Complete();
        void Rollback();
    }
}