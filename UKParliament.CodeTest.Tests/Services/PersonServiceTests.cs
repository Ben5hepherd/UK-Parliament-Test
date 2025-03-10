using Moq;
using FluentValidation;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Services;
using Xunit;
using UKParliament.CodeTest.Data.Repository;
using FluentValidation.Results;

namespace UKParliament.CodeTest.Tests.Services;

public class PersonServiceTests
{
    private readonly Mock<IRepository<Person>> _repositoryMock;
    private readonly Mock<IValidator<Person>> _validatorMock;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Person>>();
        _validatorMock = new Mock<IValidator<Person>>();
        _service = new PersonService(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task GetPersonById_ReturnsPerson_WhenPersonExists()
    {
        var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, EmailAddress = "test@test.com" };
        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(person);

        var result = await _service.GetPersonById(1);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetPersonById_ThrowsKeyNotFoundException_WhenPersonDoesNotExist()
    {
        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync((Person)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetPersonById(1));
    }

    [Fact]
    public async Task GetAllPeople_ReturnsListOfPeople()
    {
        var people = new List<Person> { new() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1 , EmailAddress = "test@test.com" } };
        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(people);

        var result = await _service.GetAllPeople();
        Assert.Single(result);
    }

    [Fact]
    public async Task AddPerson_ReturnsNewPersonId_WhenValid()
    {
        var person = new Person { FirstName = "Jane", LastName = "Doe", DateOfBirth = new DateTime(1995, 5, 5), DepartmentId = 1 , EmailAddress = "test@test.com" };

        _validatorMock
            .Setup(v => v.ValidateAsync(person, default))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.Add(person))
            .ReturnsAsync(1);

        var result = await _service.AddPerson(person);
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdatePerson_UpdatesPerson_WhenPersonExists()
    {
        var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1 , EmailAddress = "test@test.com" };

        _validatorMock
            .Setup(v => v.ValidateAsync(person, default))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.DoesEntityExist(1))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(r => r.Update(person))
            .Returns(Task.CompletedTask);

        await _service.UpdatePerson(person);

        _repositoryMock.Verify(r => r.Update(person), Times.Once);
    }

    [Fact]
    public async Task UpdatePerson_ThrowsKeyNotFoundException_WhenPersonDoesNotExist()
    {
        var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1 , EmailAddress = "test@test.com" };

        _validatorMock
            .Setup(v => v.ValidateAsync(person, default))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.DoesEntityExist(1))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdatePerson(person));
    }

    [Fact]
    public async Task DeletePerson_DeletesPerson_WhenPersonExists()
    {
        var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1 , EmailAddress = "test@test.com" };
        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(person);
        _repositoryMock.Setup(r => r.Delete(person)).Returns(Task.CompletedTask);

        await _service.DeletePerson(1);
        _repositoryMock.Verify(r => r.Delete(person), Times.Once);
    }

    [Fact]
    public async Task DeletePerson_ThrowsKeyNotFoundException_WhenPersonDoesNotExist()
    {
        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync((Person)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeletePerson(1));
    }
}
