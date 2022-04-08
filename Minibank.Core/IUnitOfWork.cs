namespace Minibank.Core
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}