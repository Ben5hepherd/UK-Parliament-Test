using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IPersonService personService, IMapper mapper) : ControllerBase
{
    [Route("{id:int}")]
    [HttpGet]
    public async Task<ActionResult<PersonViewModel>> GetById(int id)
    {
        var personEntity = await personService.GetPersonById(id);
        var personViewModel = mapper.Map<PersonViewModel>(personEntity);
        return Ok(personViewModel);
    }

    [HttpGet]
    public async Task<ActionResult<List<PersonViewModel>>> GetAll()
    {
        var personEntities = await personService.GetAllPeople();
        var personViewModels = mapper.Map<List<PersonViewModel>>(personEntities);
        return Ok(personViewModels);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] PersonViewModel personViewModel)
    {
        var personEntity = mapper.Map<Person>(personViewModel);
        var id = await personService.AddPerson(personEntity);
        return Ok(id);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] PersonViewModel personViewModel)
    {
        var personEntity = mapper.Map<Person>(personViewModel);
        await personService.UpdatePerson(personEntity);
        return Ok();
    }

    [Route("{id:int}")]
    [HttpDelete]
    public async Task<ActionResult> Delete(int id)
    {
        await personService.DeletePerson(id);
        return Ok();
    }
}