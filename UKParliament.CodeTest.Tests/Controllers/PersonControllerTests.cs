using Xunit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Tests.Controllers;

public class PersonControllerTests
{
    private readonly Mock<IPersonService> _personServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _personServiceMock = new Mock<IPersonService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new PersonController(_personServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetById_ReturnsPersonViewModel_WhenPersonExists()
    {
        var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Department = new Department { Id = 1, Name = "IT" } };
        var viewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), Department = new DepartmentViewModel { Id = 1, Name = "IT" } };

        _personServiceMock.Setup(s => s.GetPersonById(1)).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<PersonViewModel>(person)).Returns(viewModel);

        var result = await _controller.GetById(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPerson = Assert.IsType<PersonViewModel>(okResult.Value);
        Assert.Equal(1, returnedPerson.Id);
    }

    [Fact]
    public async Task GetAll_ReturnsListOfPersons()
    {
        var people = new List<Person> { new() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Department = new Department { Id = 1, Name = "IT" } } };
        var viewModels = new List<PersonViewModel> { new() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), Department = new DepartmentViewModel { Id = 1, Name = "IT" } } };

        _personServiceMock.Setup(s => s.GetAllPeople()).ReturnsAsync(people);
        _mapperMock.Setup(m => m.Map<List<PersonViewModel>>(people)).Returns(viewModels);

        var result = await _controller.GetAll();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<PersonViewModel>>(okResult.Value);
        Assert.Single(returnedList);
    }

    [Fact]
    public async Task Add_ReturnsNewPersonId()
    {
        var viewModel = new PersonViewModel { FirstName = "Jane", LastName = "Doe", DateOfBirth = new DateTime(1995, 5, 5), Department = new DepartmentViewModel { Id = 1, Name = "HR" } };
        var person = new Person { FirstName = "Jane", LastName = "Doe", DateOfBirth = new DateTime(1995, 5, 5), DepartmentId = 1, Department = new Department { Id = 1, Name = "HR" } };

        _mapperMock.Setup(m => m.Map<Person>(viewModel)).Returns(person);
        _personServiceMock.Setup(s => s.AddPerson(person)).ReturnsAsync(1);

        var result = await _controller.Add(viewModel);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(1, okResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsOk()
    {
        var viewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), Department = new DepartmentViewModel { Id = 1, Name = "IT" } };
        var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Department = new Department { Id = 1, Name = "IT" } };

        _mapperMock.Setup(m => m.Map<Person>(viewModel)).Returns(person);
        _personServiceMock.Setup(s => s.UpdatePerson(person)).Returns(Task.CompletedTask);

        var result = await _controller.Update(viewModel);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk()
    {
        _personServiceMock.Setup(s => s.DeletePerson(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);
        Assert.IsType<OkResult>(result);
    }
}
