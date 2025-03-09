using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Data.Repository
{
    public interface IRepository
    {
        Task<T?> GetById<T>(int id) where T : Entity;
        Task<List<T>> GetAll<T>() where T : Entity;


        Task<int> Add<T>(T entity) where T : Entity;


        Task<bool> DoesEntityExist<T>(int id) where T : Entity;
        Task Update<T>(T entity) where T : Entity;


        Task Delete<T>(T entity) where T : Entity;
    }
}
