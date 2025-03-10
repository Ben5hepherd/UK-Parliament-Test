using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests.Controllers;

public class DepartmentControllerTests
{
    private readonly Mock<IDepartmentService> _departmentServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DepartmentController _controller;

    public DepartmentControllerTests()
    {
        _departmentServiceMock = new Mock<IDepartmentService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new DepartmentController(_departmentServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsListOfDepartmentViewModels()
    {
        // Arrange
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "HR" },
            new() { Id = 2, Name = "IT" }
        };
        var departmentViewModels = new List<DepartmentViewModel>
        {
            new() { Id = 1, Name = "HR" },
            new() { Id = 2, Name = "IT" }
        };

        _departmentServiceMock.Setup(s => s.GetAllDepartments()).ReturnsAsync(departments);
        _mapperMock.Setup(m => m.Map<List<DepartmentViewModel>>(departments)).Returns(departmentViewModels);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDepartments = Assert.IsType<List<DepartmentViewModel>>(actionResult.Value);
        Assert.Equal(2, returnedDepartments.Count);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoDepartmentsExist()
    {
        // Arrange
        _departmentServiceMock.Setup(s => s.GetAllDepartments()).ReturnsAsync(new List<Department>());
        _mapperMock.Setup(m => m.Map<List<DepartmentViewModel>>(It.IsAny<List<Department>>())).Returns(new List<DepartmentViewModel>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDepartments = Assert.IsType<List<DepartmentViewModel>>(actionResult.Value);
        Assert.Empty(returnedDepartments);
    }
}

