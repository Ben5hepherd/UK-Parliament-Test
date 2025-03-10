using Moq;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository;
using UKParliament.CodeTest.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IRepository<Department>> _repositoryMock;
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Department>>();
        _service = new DepartmentService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllDepartments_ReturnsListOfDepartments()
    {
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "HR" },
            new() { Id = 2, Name = "IT" }
        };

        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(departments);

        var result = await _service.GetAllDepartments();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Name == "HR");
        Assert.Contains(result, d => d.Name == "IT");
    }

    [Fact]
    public async Task GetAllDepartments_ReturnsEmptyList_WhenNoDepartmentsExist()
    {
        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Department>());

        var result = await _service.GetAllDepartments();

        Assert.Empty(result);
    }
}
