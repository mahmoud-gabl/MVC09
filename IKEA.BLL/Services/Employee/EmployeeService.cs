using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Common.Services;
using IKEA.BLL.Models.Employee;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Repositories.Employees;
using IKEA.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public EmployeeService(IUnitOfWork unitOfWork,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }
        public async  Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(string search)
        {
            return  await _unitOfWork.EmployeeRepository.GetAllAsQuerable().
                Where(E => !E.IsDeleted && (string.IsNullOrEmpty(search) || E.Name.ToLower().Contains(search.ToLower())))
                .Include(E => E.Department)

                .Select(employee => new EmployeeDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    IsActive = employee.IsActive,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    Gender = employee.Gender.ToString(),
                    EmployeeType = employee.EmployeeType.ToString(),
                    Department = employee.Department.Name,
                }).ToListAsync();
        }

        public  async Task<EmployeeDetailsDto?> GetEmployeeByIdAsync(int id)
        {
            var employee =  await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee is { })
                return new EmployeeDetailsDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    HiringDate = employee.HiringDate,
                    //--------------------------
                    Gender = employee.Gender,
                    EmployeeType = employee.EmployeeType,
                    //--------------------------
                    Department = employee.Department.Name,
                    Image = employee.Image



                };
            return null;
        }
        public async  Task<int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto)
        {
            var employee = new IKEA.DAL.Models.Employees.Employee()
            {
                Name=employeeDto.Name,
                Age= employeeDto.Age,
                Address= employeeDto.Address,
                IsActive= employeeDto.IsActive,
                Salary= employeeDto.Salary,
                Email= employeeDto.Email,
                PhoneNumber= employeeDto.PhoneNumber,
                HiringDate= employeeDto.HiringDate,
                Gender= employeeDto.Gender,
                EmployeeType= employeeDto.EmployeeType,
                DepartmentId= employeeDto.DepartmentId,
                CreatedBy=1,
                LastModificationBy=1,
                LastModificationOn= DateTime.UtcNow
            };
             if (employeeDto.Image is not null)
            {
                employee.Image = _attachmentService.UploadFile(employeeDto.Image, "images");
            }
            _unitOfWork.EmployeeRepository. Add(employee);
            return  await _unitOfWork.CompleteAsync();
        }
        public   async Task<int> UpdateEmployeeAsync(UpdatedEmployeeDto employeeDto)
        {
            var employee = new IKEA.DAL.Models.Employees.Employee()
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId= employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow
            };
           _unitOfWork.EmployeeRepository.Update(employee);
            return await  _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employeeRepo = _unitOfWork.EmployeeRepository;
            var employee =  await employeeRepo.GetByIdAsync(id);
             if (employee is { })
            {
                 employeeRepo.Delete(employee);
            }
            return  await _unitOfWork.CompleteAsync() > 0;
        }

       

        
    }
}
