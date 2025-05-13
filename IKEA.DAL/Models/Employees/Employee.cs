using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Common.Enum;
using IKEA.DAL.Models.Departments;

namespace IKEA.DAL.Models.Employees
{
    public class Employee:ModelBase
    {
        public string Name { get; set; } = null!;
        public int?Age { get; set; }
        public string? Address { get; set; }
         public decimal? Salary { get; set; }
        public bool IsActive { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
         public DateTime HiringDate { get; set; }
         public Gender Gender { get; set; }
        public EmployeeType EmployeeType { get; set; }
        #region Department
        public int? DepartmentId { get; set; }
        //Vavigational Property [One]
        public virtual Department? Department { get; set; }
        #endregion
        public string? Image { get; set; }



    }
}
