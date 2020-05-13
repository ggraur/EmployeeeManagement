using EmployeeeManagement.Models;
using EmployeeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using EmployeeeManagement.Security;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EmployeeeManagement.Controllers
{

    [Authorize]
    public class HomeController : Controller

    {
        private readonly IEmployeeRepository _employeeRepository;
     
        [Obsolete]
        protected readonly IHostingEnvironment hostingEnvironment;

        private readonly ILogger<HomeController> _logger;

        private readonly IDataProtector protector;/*video 120*/

        [Obsolete]
        public HomeController(IEmployeeRepository employeeRepository,
                              IHostingEnvironment hostingEnvironment,
                              ILogger<HomeController> logger,
                              IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
             
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this._logger = logger;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue); /*video 120*/
        }

        [AllowAnonymous]
        public ViewResult Index()
        {

            var model = _employeeRepository.GetAllEmployee()
                        .Select(e => 
                        {
                            e.EncryptedId = protector.Protect(e.Id.ToString());
                            return e;
                        });
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Details(string id)
        {
            //throw new Exception("Error in details view");
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Log Warning");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");

            int employeeId = Convert.ToInt32(protector.Unprotect(id));


            Employee employee = _employeeRepository.GetEmployee(employeeId);
            if (employee==null) 
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", employeeId);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = employee,
             //   Employee = _employeeRepository.GetEmployee(id??1),
                PageTitle = "Employee Details"
            };
  
            return View(homeDetailsViewModel);
           
        }
        [HttpGet]
        //[Authorize]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
       // [Authorize]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath=employee.PhotoPath
            };

            return View(employeeEditViewModel);
        }


        [HttpPost]
      //  [Obsolete]
      //  [Authorize]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photos != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string uploadsFolder = "D:\\GIT\\Test\\EmployeeeManagement\\wwwroot";

                        string filePath=Path.Combine(uploadsFolder,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploatedFile(model);
                }
               
                _employeeRepository.Update(employee);
                return RedirectToAction("index");

                //EmployeeCreateViewModel newEmployee = _employeeRepository.Add(model);
                //return RedirectToAction("details", new { id = newEmployee.Id });
            }

            return View();
        }

        private static string ProcessUploatedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string uploadsFolder = "D:\\GIT\\Test\\EmployeeeManagement\\wwwroot\\images";//System.IO.Path.GetFullPath(hostEnvironment.WebRootPath, "images"); // Path.Combine(hostEnvironment.WebRootPath, "images");

                    uniqueFileName = '{' + Guid.NewGuid().ToString() + "}_" + photo.FileName;

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                        photo.CopyTo(fileStream);
                    }
                        
                }
            }

            return uniqueFileName;
        }

        [HttpPost]
     //   [Obsolete]
        //[Authorize]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploatedFile(model);

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details",new { id=newEmployee.Id});

                //EmployeeCreateViewModel newEmployee = _employeeRepository.Add(model);
                //return RedirectToAction("details", new { id = newEmployee.Id });
            }

            return View();
        }

    }
}
