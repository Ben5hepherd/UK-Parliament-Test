using FluentValidation;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository;

namespace UKParliament.CodeTest.Services;

public class PersonService(IRepository repository, IValidator<Person> validator) : IPersonService
{
    public async Task<Person> GetPersonById(int id)
    {
        var person = await repository.GetById<Person>(id);
        return person ?? throw PersonNotFoundException(id);
    }

    public async Task<List<Person>> GetAllPeople()
    {
        return await repository.GetAll<Person>();
    }

    public async Task<int> AddPerson(Person person)
    {
        await validator.ValidateAndThrowAsync(person);
        return await repository.Add(person);
    }

    public async Task UpdatePerson(Person person)
    {
        await validator.ValidateAndThrowAsync(person);

        var doesPersonExist = await repository.DoesEntityExist<Person>(person.Id);

        if (doesPersonExist)
        {
            await repository.Update(person);
        }
        else
        {
            throw PersonNotFoundException(person.Id);
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
            throw PersonNotFoundException(id);
        }
    }

    private static KeyNotFoundException PersonNotFoundException(int id)
    {
        return new KeyNotFoundException($"Person with ID {id} does not exist");
    }
}