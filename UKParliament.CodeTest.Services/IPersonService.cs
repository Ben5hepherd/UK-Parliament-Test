using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<Person> GetPersonById(int id);
    Task<List<Person>> GetAllPeople();
    Task<int> AddPerson(Person person);
    Task UpdatePerson(Person person);
    Task DeletePerson(int id);
}