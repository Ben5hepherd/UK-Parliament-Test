using FluentValidation;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository;

namespace UKParliament.CodeTest.Services;

public class PersonService(IRepository<Person> repository, IValidator<Person> validator) : IPersonService
{
    public async Task<Person> GetPersonById(int id)
    {
        var person = await repository.GetById(id);
        return person ?? throw PersonNotFoundException(id);
    }

    public async Task<List<Person>> GetAllPeople()
    {
        return await repository.GetAll();
    }

    public async Task<int> AddPerson(Person person)
    {
        await validator.ValidateAndThrowAsync(person);
        return await repository.Add(person);
    }

    public async Task UpdatePerson(Person person)
    {
        await validator.ValidateAndThrowAsync(person);

        var doesPersonExist = await repository.DoesEntityExist(person.Id);

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
        var personEntity = await repository.GetById(id);
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