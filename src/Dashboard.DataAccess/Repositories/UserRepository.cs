using Dashboard.Core.Repositories;
using Dashboard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public class UserRepository : ICrudRepository<UserModel>
{
    private readonly DashboardDBContext _context;

    public UserRepository(DashboardDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task Create(UserModel user)
    {
        return Task.FromResult(_context.Set<UserModel>().Add(user).Entity);
    }

    public Task Delete(UserModel user)
    {
        _context.Set<UserModel>().Remove(user);
        _context.Entry(user).State = EntityState.Deleted;
        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }

    public async Task<UserModel> ReadById(string userId)
    {
        return await _context.Set<UserModel>().AsNoTracking().Where(x => x.Id == userId).FirstAsync();
    }

    public async Task<IEnumerable<UserModel>> ReadAll()
    {
        return await _context.Set<UserModel>().AsNoTracking().ToListAsync();
    }

    public Task Update(UserModel user)
    {
        _context.Set<UserModel>().Attach(user);
        _context.Entry(user).State = EntityState.Modified;
        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }
}