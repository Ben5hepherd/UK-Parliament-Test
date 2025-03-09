using FluentValidation;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Services.Validators

{ 
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(person => person.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .Length(1, 100).WithMessage("First Name must be between 1 and 100 characters.");

            RuleFor(person => person.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Length(1, 100).WithMessage("Last Name must be between 1 and 100 characters.");

            RuleFor(person => person.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");

            RuleFor(person => person.DepartmentId)
                .GreaterThan(0).WithMessage("Department Id is required and must be greater than zero.");
        }
    }
}
