using AutoMapper;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonViewModel>();

        CreateMap<PersonViewModel, Person>()
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Department.Id))
            .ForMember(dest => dest.Department, opt => opt.Ignore());

        CreateMap<Department, DepartmentViewModel>();
    }
}
