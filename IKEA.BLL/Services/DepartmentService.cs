using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public  async Task<IEnumerable<DepartmentToReturnDto>> GetAllDepartmentsAsync()
        {
            var departments =  await _unitOfWork.DepartmentRepository.GetAllAsQuerable().Select(department=> new DepartmentToReturnDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                
                CreationDate = department.CreationDate
            }).AsNoTracking().ToListAsync();




            return departments;
        }

        public async Task<DepartmentDetailsToReturnDto?> GetDepartmentByIdAsync(int id)
        {
            var department =  await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
             if (department is { })
            {
                return new DepartmentDetailsToReturnDto()
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    Description = department.Description,

                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModificationBy = department.LastModificationBy,
                    LastModificationOn = department.LastModificationOn,

                };
            }
            return null;
            
        }
        public async  Task<int> CreateDerpartmenetAsync(CreatedDepartmentDto departmentDto)
        {
            var Createddepartment = new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                CreatedBy = 1,
                LastModificationBy = 1,
                LastModificationOn= DateTime.UtcNow,
            };
              _unitOfWork.DepartmentRepository.Add(Createddepartment);
            return  await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto)
        {
            var UpdatedDepartment = new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,

                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow,
            };
           _unitOfWork.DepartmentRepository.Update(UpdatedDepartment);
            return  await _unitOfWork.CompleteAsync();
        }
        public async Task<bool> DeletedDepartmentAsync(int id)
        {
            var departmentRepo = _unitOfWork.DepartmentRepository;
            var department =  await departmentRepo.GetByIdAsync(id);
             if (department is not null)
            {
                 departmentRepo.Delete(department);
            }
            return  await _unitOfWork.CompleteAsync() > 0;
        }

        
    }
}
