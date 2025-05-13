using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models;
using IKEA.DAL.Models.Departments;

namespace IKEA.BLL.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentToReturnDto>> GetAllDepartmentsAsync();
       
        Task<DepartmentDetailsToReturnDto?> GetDepartmentByIdAsync(int id);
        Task<int> CreateDerpartmenetAsync(CreatedDepartmentDto departmentDto);
        Task<int> UpdateDepartmentAsync(UpdateDepartmentDto department);
        Task<bool> DeletedDepartmentAsync(int id);


    }
}
