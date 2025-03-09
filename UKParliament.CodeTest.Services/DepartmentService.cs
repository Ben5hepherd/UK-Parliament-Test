using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository;

namespace UKParliament.CodeTest.Services
{
    public class DepartmentService(IRepository repository) : IDepartmentService
    {
        public async Task<List<Department>> GetAllDepartments()
        {
            return await repository.GetAll<Department>();
        }
    }
}
