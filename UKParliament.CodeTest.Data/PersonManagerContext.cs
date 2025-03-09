using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Data;

public class PersonManagerContext : DbContext
{
    public PersonManagerContext(DbContextOptions<PersonManagerContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Department)
            .WithMany()
            .HasForeignKey(p => p.DepartmentId)
            .IsRequired();

        modelBuilder.Entity<Person>()
            .Navigation(p => p.Department)
            .AutoInclude();

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });

        modelBuilder.Entity<Person>().HasData(
            new Person { Id = 1, FirstName = "Lewis", LastName = "Hamilton", DateOfBirth = new DateTime(1985, 1, 7), DepartmentId = 1 },
            new Person { Id = 2, FirstName = "George", LastName = "Russell", DateOfBirth = new DateTime(1998, 2, 15), DepartmentId = 3 },
            new Person { Id = 3, FirstName = "Lando", LastName = "Norris", DateOfBirth = new DateTime(1999, 11, 13), DepartmentId = 4 });
    }

    public DbSet<Person> People { get; set; }

    public DbSet<Department> Departments { get; set; }
}