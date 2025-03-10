using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController(IDepartmentService departmentService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PersonViewModel>> GetAll()
    {
        var departmentEntities = await departmentService.GetAllDepartments();
        var departmentViewModels = mapper.Map<List<DepartmentViewModel>>(departmentEntities);
        return Ok(departmentViewModels);
    }
}