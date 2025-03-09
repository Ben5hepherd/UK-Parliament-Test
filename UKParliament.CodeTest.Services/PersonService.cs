using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository;

namespace UKParliament.CodeTest.Services;

public class PersonService(IRepository repository) : IPersonService
{
    public async Task<Person?> GetPersonById(int id)
    {
        return await repository.GetById<Person>(id);
    }

    public async Task<List<Person>> GetAllPeople()
    {
        return await repository.GetAll<Person>();
    }

    public async Task<int> AddPerson(Person person)
    {
        return await repository.Add(person);
    }

    public async Task UpdatePerson(Person person)
    {
        var doesPersonExist = await repository.DoesEntityExist<Person>(person.Id);

        if (doesPersonExist)
        {
            await repository.Update(person);
        }
        else
        {
            throw new Exception("Person does not exist");
        }
    }

    public async Task DeletePerson(int id)
    {
        var personEntity = await repository.GetById<Person>(id);
        if (personEntity != null)
        {
            await repository.Delete(personEntity);
        }
        else
        {
            throw new Exception("Person does not exist");
        }
    }
}