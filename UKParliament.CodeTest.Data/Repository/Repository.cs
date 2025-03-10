using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Data.Repository;

public class Repository<T>(PersonManagerContext dbContext) : IRepository<T> where T : notnull, Entity
{
    public async Task<T?> GetById(int id)
    {
        return await dbContext.FindAsync<T>(id);
    }

    public async Task<List<T>> GetAll()
    {
        return await dbContext.Set<T>().ToListAsync();
    }

    public async Task<int> Add(T entity)
    {
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> DoesEntityExist(int id)
    {
        return await dbContext.Set<T>().AnyAsync(e => e.Id == id);
    }

    public async Task Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
