using Dashboard.Core.Repositories;
using Dashboard.DataAccess.Repositories;
using System.Threading.Tasks;
using Dashboard.Core.Models;

namespace Dashboard.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {

    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DashboardDBContext _context;
        private UserRepository _userRepository;

        public ICrudRepository<UserModel> Users => _userRepository ??= new UserRepository(_context);

        public UnitOfWork(DashboardDBContext context) => _context = context;

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
