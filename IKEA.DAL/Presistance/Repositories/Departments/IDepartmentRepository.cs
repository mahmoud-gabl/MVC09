using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Presistance.Repositories._Generic;

namespace IKEA.DAL.Presistance.Repositories.Departments
{
    public interface IDepartmentRepository:IGenericRepository<Department>
    {
       

    }
}
