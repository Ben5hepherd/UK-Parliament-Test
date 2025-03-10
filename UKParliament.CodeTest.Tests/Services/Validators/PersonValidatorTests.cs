using FluentValidation.TestHelper;
using Moq;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository;
using UKParliament.CodeTest.Services.Validators;
using Xunit;

namespace UKParliament.CodeTest.Tests.Services.Validators;

public class PersonValidatorTests
{
    private readonly PersonValidator _validator;
    private readonly Mock<IRepository<Person>> _repositoryMock;

    public PersonValidatorTests()
    {
        _repositoryMock = new Mock<IRepository<Person>>();
        _validator = new PersonValidator(_repositoryMock.Object);
    }

    [Fact]
    public async Task Validate_ShouldPass_WhenPersonIsValid()
    {
        var person = new Person
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            DepartmentId = 1,
            EmailAddress = "test@test.com"
        };
        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        var result = await _validator.TestValidateAsync(person);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenFirstNameIsEmpty()
    {
        var person = new Person { FirstName = "", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, EmailAddress = "test@test.com" };
        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        var result = await _validator.TestValidateAsync(person);
        
        result.ShouldHaveValidationErrorFor(p => p.FirstName).WithErrorMessage("First Name is required.");
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenLastNameIsEmpty()
    {
        var person = new Person { FirstName = "John", LastName = "", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, EmailAddress = "test@test.com" };
        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        var result = await _validator.TestValidateAsync(person);
        
        result.ShouldHaveValidationErrorFor(p => p.LastName).WithErrorMessage("Last Name is required.");
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenEmailIsEmpty()
    {
        var person = new Person { EmailAddress = string.Empty, FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now.AddYears(-25), DepartmentId = 1 };
        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        var result = await _validator.TestValidateAsync(person);

        result.ShouldHaveValidationErrorFor(x => x.EmailAddress).WithErrorMessage("Email Address is required.");
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenEmailIsInvalidFormat()
    {
        var person = new Person { EmailAddress = "invalid-email", FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now.AddYears(-25), DepartmentId = 1 };
        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        var result = await _validator.TestValidateAsync(person);

        result.ShouldHaveValidationErrorFor(x => x.EmailAddress).WithErrorMessage("Invalid email format.");
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenEmailExistsAlready()
    {
        var person = new Person { Id = 1, EmailAddress = "test@example.com", FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now.AddYears(-25), DepartmentId = 1 };
        var existingPerson = new Person { Id = 2, EmailAddress = "test@example.com", FirstName = "Jane", LastName = "Doe", DateOfBirth = DateTime.Now.AddYears(-30), DepartmentId = 2 };
        _repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync([existingPerson]);

        var result = await _validator.TestValidateAsync(person);

        Assert.False(result.IsValid);
        Assert.Equal("Email address is already in use.", result.Errors.Single().ErrorMessage);
    }

    [Fact]
    public async Task Validate_ShouldPass_WhenEmailIsUnique()
    {
        var person = new Person { Id = 1, EmailAddress = "unique@example.com", FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now.AddYears(-25), DepartmentId = 1 };
        var existingPerson = new Person { Id = 2, EmailAddress = "other@example.com", FirstName = "Jane", LastName = "Doe", DateOfBirth = DateTime.Now.AddYears(-30), DepartmentId = 2 };
        _repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Person> { existingPerson });

        var result = await _validator.TestValidateAsync(person);

        result.ShouldNotHaveValidationErrorFor(x => x.EmailAddress);
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenDateOfBirthIsInTheFuture()
    {
        var person = new Person { FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now.AddDays(1), DepartmentId = 1, EmailAddress = "test@test.com" };
        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        var result = await _validator.TestValidateAsync(person);
        
        result.ShouldHaveValidationErrorFor(p => p.DateOfBirth).WithErrorMessage("Date of Birth must be in the past.");
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenDepartmentIdIsZeroOrNegative()
    {
        var person = new Person { FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 0 , EmailAddress = "test@test.com" };
        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        var result = await _validator.TestValidateAsync(person);
        
        result.ShouldHaveValidationErrorFor(p => p.DepartmentId).WithErrorMessage("Department Id is required and must be greater than zero.");
    }
}
