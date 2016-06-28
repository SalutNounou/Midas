namespace Midas.Model
{
    public interface IUnitOfWork
    {
        IRepository<Security> Securities { get; }
        int Complete();
        void Rollback();
    }
}