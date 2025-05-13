using System.Linq.Expressions;
using System.Security.Policy;
using AutoMapper;
using IKEA.BLL.Models;
using IKEA.BLL.Services;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;


        #region Services
        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IWebHostEnvironment environment, IMapper mapper)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
            _mapper = mapper;
            _environment = environment;
        }
        #endregion
        #region Index
        [HttpGet]
        //BaseUrl/Department/Index
        public async Task<IActionResult> Index()
        {
            var departments =  await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }
        #endregion
        #region Create
        #region Get
        [HttpGet]
         public  async Task<IActionResult> Create()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create(CreatedDepartmentDto department)
        {
            if (!ModelState.IsValid)
                return View(department);
            var message = string.Empty;
            try
            {
                var result = await  _departmentService.CreateDerpartmenetAsync(department);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Sorry The Department Has Not Been Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(department);
                }
                 
            }
            
            catch(Exception ex)
            {
                //1- Log Exception
                _logger.LogError(ex, ex.Message);
                //2- Set Frindly Message
                if(_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(department);
                }
                else
                {
                    message = "Sorry The Department Has Not Been Created";
                    return View("Error", message);
                }

            }
        }
        #endregion
        #endregion
        #region Details
        [HttpGet]
         public async  Task<IActionResult> Details(int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            var department =  await _departmentService.GetDepartmentByIdAsync(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(department);

        }
        #endregion
        #region  Edit
        #region Get
        [HttpGet]
         public async  Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400

            }
            var department =  await _departmentService.GetDepartmentByIdAsync(id.Value);
            if(department is null)
            {
                return NotFound();//404
            }
            var departmentVm = _mapper.Map<DepartmentDetailsToReturnDto, DepartmentEditViewModel>(department);
            //var viewModel = new DepartmentEditViewModel()
            //{
            //    Id = department.Id,
            //    Code = department.Code,
            //    Name = department.Name,
            //    Description = department.Description,
            //    CreationDate = department.CreationDate
            //};
            return View(departmentVm);

        }
        #endregion
        #region  Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Edit (int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                var updatedDepartment = _mapper.Map<UpdateDepartmentDto>(departmentVM);
                //var updatedDepartment = new UpdateDepartmentDto()
                //{
                //    Id = id,
                //    Code = departmentVM.Code,
                //    Name = departmentVM.Name,
                //    Description = departmentVM.Description,
                //    CreationDate = departmentVM.CreationDate


                //};
                var updated =  await _departmentService.UpdateDepartmentAsync(updatedDepartment)>0;
                if(updated)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry , An Error Occured While Updating The Department";

            }
             catch(Exception ex)
            {
                //1-
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "Sorry , An Error Occured While Updating The Department";

            }
            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);


        }
        #endregion
        #endregion
        #region Delete
        [HttpGet]
         public  async Task<IActionResult> Delete (int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            var department =  await _departmentService.GetDepartmentByIdAsync(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(department);
        }
        //---------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted =  await _departmentService.DeletedDepartmentAsync(id);
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));

                }
                message = "An Error Ocurred During Deleting The Department";
            }
             catch(Exception ex)
            {
                //1-
                _logger.LogError(ex, ex.Message);
                //2- 
                message = _environment.IsDevelopment() ? ex.Message : "An Error Ocurred During Deleting The Department";


            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


    } 
}
