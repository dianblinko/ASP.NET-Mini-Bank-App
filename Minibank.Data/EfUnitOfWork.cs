using System.Threading.Tasks;
using Minibank.Core;
using Minibank.Data.Context;

namespace Minibank.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly MinibankContext _context;

        public EfUnitOfWork(MinibankContext context)
        {
            _context = context;
        }

        public Task<int> SaveChanges()
        { 
            return _context.SaveChangesAsync();
        }
    }
}