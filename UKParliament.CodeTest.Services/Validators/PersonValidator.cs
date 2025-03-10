using FluentValidation;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository;

namespace UKParliament.CodeTest.Services.Validators;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator(IRepository<Person> personRepository)
    {
        RuleFor(person => person.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .Length(1, 100).WithMessage("First Name must be between 1 and 100 characters.");

        RuleFor(person => person.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .Length(1, 100).WithMessage("Last Name must be between 1 and 100 characters.");


        RuleFor(person => person.EmailAddress)
            .NotEmpty().WithMessage("Email Address is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(person => person)
            .MustAsync(async (person, _) =>
            {
                var allPersons = await personRepository.GetAll();
                var otherPersons = allPersons.Where(p => p.Id != person.Id);
                var isEmailAddressUnique = otherPersons.All(p => p.EmailAddress != person.EmailAddress);
                return isEmailAddressUnique;
            })
            .WithMessage("Email address is already in use.");

        RuleFor(person => person.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required.")
            .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");

        RuleFor(person => person.DepartmentId)
            .GreaterThan(0).WithMessage("Department Id is required and must be greater than zero.");
    }
}
