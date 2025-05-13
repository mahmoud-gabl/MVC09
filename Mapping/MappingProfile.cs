using AutoMapper;
using IKEA.BLL.Models;
using IKEA.PL.Models.Departments;

namespace IKEA.PL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DepartmentDetailsToReturnDto, DepartmentEditViewModel>();
            CreateMap<DepartmentEditViewModel, UpdateDepartmentDto>();
        
        }

    }
}
