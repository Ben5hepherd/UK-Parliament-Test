using FluentValidation.TestHelper;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Services.Validators;
using Xunit;

namespace UKParliament.CodeTest.Tests.Services.Validators;

public class PersonValidatorTests
{
    private readonly PersonValidator _validator;

    public PersonValidatorTests()
    {
        _validator = new PersonValidator();
    }

    [Fact]
    public void Validate_ShouldPass_WhenPersonIsValid()
    {
        var person = new Person
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            DepartmentId = 1
        };

        var result = _validator.TestValidate(person);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_ShouldFail_WhenFirstNameIsEmpty()
    {
        var person = new Person { FirstName = "", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1 };
        
        var result = _validator.TestValidate(person);
        
        result.ShouldHaveValidationErrorFor(p => p.FirstName).WithErrorMessage("First Name is required.");
    }

    [Fact]
    public void Validate_ShouldFail_WhenLastNameIsEmpty()
    {
        var person = new Person { FirstName = "John", LastName = "", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1 };
        
        var result = _validator.TestValidate(person);
        
        result.ShouldHaveValidationErrorFor(p => p.LastName).WithErrorMessage("Last Name is required.");
    }

    [Fact]
    public void Validate_ShouldFail_WhenDateOfBirthIsInTheFuture()
    {
        var person = new Person { FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now.AddDays(1), DepartmentId = 1 };
        
        var result = _validator.TestValidate(person);
        
        result.ShouldHaveValidationErrorFor(p => p.DateOfBirth).WithErrorMessage("Date of Birth must be in the past.");
    }

    [Fact]
    public void Validate_ShouldFail_WhenDepartmentIdIsZeroOrNegative()
    {
        var person = new Person { FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 0 };
        
        var result = _validator.TestValidate(person);
        
        result.ShouldHaveValidationErrorFor(p => p.DepartmentId).WithErrorMessage("Department Id is required and must be greater than zero.");
    }
}
