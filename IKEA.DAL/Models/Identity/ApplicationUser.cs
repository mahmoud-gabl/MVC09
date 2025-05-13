using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IKEA.DAL.Models.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
             public bool IsAgree { get; set; }
    }
}
