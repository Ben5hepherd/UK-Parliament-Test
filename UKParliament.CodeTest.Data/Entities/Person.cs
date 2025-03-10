namespace UKParliament.CodeTest.Data.Entities;

public class Person : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    public required DateTime DateOfBirth { get; set; }

    public required int DepartmentId { get; set; }
    public Department? Department { get; set; }
}