using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Services;

public interface IDepartmentService
{
    Task<List<Department>> GetAllDepartments();
}
