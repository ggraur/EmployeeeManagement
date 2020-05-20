using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeeManagement.Controllers
{
    public class ItemController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public static ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        //{ 
        //}

    }
}
