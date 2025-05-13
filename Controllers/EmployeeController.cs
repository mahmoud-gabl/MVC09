using IKEA.BLL.Models;
using IKEA.BLL.Models.Employee;
using IKEA.BLL.Services;
using IKEA.BLL.Services.Employee;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        #region Services

        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmployeeController> _logger;
       

        public EmployeeController(
            IEmployeeService employeeService,
            IWebHostEnvironment webHostEnvironment,
            ILogger<EmployeeController> logger
           
            ) // ASK CLR for Creating Object from EmployeeService Implicitly
        {
            _logger = logger;
            
            _webHostEnvironment = webHostEnvironment;
            _employeeService = employeeService;
        }

        #endregion
        #region Index
        [HttpGet]
        //BaseUrl/Employee/Index
        public  async Task<IActionResult> Index(string search)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(search);
            return View(employees);
        }
        #endregion
        #region Create
        #region Get
        [HttpGet]
        public  async Task<IActionResult> Create([FromServices] IDepartmentService departmentService)
        {
            ViewData["Departments"] =  await departmentService.GetAllDepartmentsAsync();
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Create(CreatedEmployeeDto employee)
        {
            if (!ModelState.IsValid)
                return View(employee);
            var message = string.Empty;
            try
            {
                var result =  await _employeeService.CreateEmployeeAsync(employee);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Sorry The Employee Has Not Been Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(employee);
                }

            }

            catch (Exception ex)
            {
                //1- Log Exception
                _logger.LogError(ex, ex.Message);
                //2- Set Frindly Message
                if (_webHostEnvironment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(employee);
                }
                else
                {
                    message = "Sorry The Employee Has Not Been Created";
                    return View("Error", message);
                }

            }
        }
        #endregion
        #endregion
        #region Details
        [HttpGet]
        public  async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee = await  _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
            return View(employee);

        }
        #endregion
        #region  Edit
        #region Get
        [HttpGet]
        public  async Task<IActionResult> Edit(int? id, [FromServices] IDepartmentService departmentService)
        {
            if (id is null)
            {
                return BadRequest();//400

            }
            var employee =  await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound();//404
            }
            ViewData["Departments"] =  await departmentService.GetAllDepartmentsAsync();
            return View(new UpdatedEmployeeDto()
            {
                Name= employee.Name,
                Address= employee.Address,
                Email= employee.Email,
                Age= employee.Age,
                Salary= employee.Salary,
                PhoneNumber= employee.PhoneNumber,
                IsActive=employee.IsActive,
                EmployeeType= employee.EmployeeType,
                Gender= employee.Gender,
                HiringDate= employee.HiringDate

            });

        }
        #endregion
        #region Post
        [HttpPost] // POST
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Edit([FromRoute] int id, UpdatedEmployeeDto employee)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(employee);

            var message = string.Empty;

            try
            {
                var updated = await  _employeeService.UpdateEmployeeAsync(employee) > 0;
                if (updated)
                    return RedirectToAction(nameof(Index));

                message = "Employee is not Updated";
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Set Message

                _logger.LogError(ex, ex.Message);

                if (_webHostEnvironment.IsDevelopment())
                    message = ex.Message;
                else
                    message = "The Employee is not Created";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(employee);

        }

        #endregion
        #endregion
        #region Delete
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {

                var deleted =  await _employeeService.DeleteEmployeeAsync(id);
                if (deleted)
                    return RedirectToAction(nameof(Index));
                message = "An Error Occured  During The Deleting Of Employee";
            }
            catch (Exception ex)
            {
                //1- Log Exception
                _logger.LogError(ex, ex.Message);
                //2- Set Message
                message = _webHostEnvironment.IsDevelopment() ? ex.Message : "Sorry An Error Occured During Deleting  The Department :(";
            }
            return RedirectToAction(nameof(Index));


        }
        #endregion
        #endregion
    }
}
