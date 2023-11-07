using Dashboard.Core.Repositories;
using Dashboard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public class TaskRepository : ICrudRepository<TaskModel>
{
    private readonly DashboardDBContext _context;

    public TaskRepository(DashboardDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task Create(TaskModel taskModel)
    {
        _context.Set<TaskModel>().Add(taskModel);
        _context.Entry(taskModel).State = EntityState.Added;
        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }

    public Task Delete(TaskModel taskModel)
    {
        _context.Set<TaskModel>().Remove(taskModel);
        _context.Entry(taskModel).State = EntityState.Deleted;
        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<TaskModel>> ReadAll()
    {
        return await _context.Set<TaskModel>().AsNoTracking().ToListAsync();
    }

    public async Task<TaskModel> ReadById(string taskId)
    {
        return await _context.Set<TaskModel>().AsNoTracking().Where(x => x.Id == Int32.Parse(taskId)).FirstAsync();
    }

    public Task Update(TaskModel taskModel)
    {
        _context.Set<TaskModel>().Attach(taskModel);
        _context.Entry(taskModel).State = EntityState.Modified;
        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }
}