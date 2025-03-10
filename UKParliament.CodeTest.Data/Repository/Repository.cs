using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Data.Repository;

public class Repository(PersonManagerContext dbContext) : IRepository
{
    public async Task<T?> GetById<T>(int id) where T : Entity
    {
        return await dbContext.FindAsync<T>(id);
    }

    public async Task<List<T>> GetAll<T>() where T : Entity
    {
        return await dbContext.Set<T>().ToListAsync();
    }

    public async Task<int> Add<T>(T entity) where T : Entity
    {
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> DoesEntityExist<T>(int id) where T : Entity
    {
        return await dbContext.Set<T>().AnyAsync(e => e.Id == id);
    }

    public async Task Update<T>(T entity) where T : Entity
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete<T>(T entity) where T : Entity
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
