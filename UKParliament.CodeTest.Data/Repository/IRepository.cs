using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Data.Repository;

public interface IRepository<T> where T : notnull, Entity
{
    Task<T?> GetById(int id);
    Task<List<T>> GetAll();
    Task<int> Add(T entity);
    Task<bool> DoesEntityExist(int id);
    Task Update(T entity);
    Task Delete(T entity);
}
