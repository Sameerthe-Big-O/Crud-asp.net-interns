using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using StudentsDataApp.DAL;
using StudentsDataApp.Models;

namespace StudentsDataApp.Controllers
{
    public class ClassController : Controller
    {
        private readonly ClassDataAccessLayer dal;

        public ClassController()
        {
            dal = new ClassDataAccessLayer();
        }

       
        [Obsolete]
        public  JsonResult GetAllClasses()
        {
            List<Class> clslist = dal.GetAllClasses();
            return Json(clslist);
            
        }

        public IActionResult create()
        {
            return View(); 
        }
        [HttpPost]
        [Obsolete]
        public JsonResult Create(Class cls)
        {
            if (dal.ClassExists(cls)) 
            {
                return Json(new { success = false, message = "This class name already exists. Try a different class name." });
            }

            dal.AddClass(cls);
            return Json(new { success = true, message = "Class added successfully!" });
        }

        [HttpGet]
        [Obsolete]
        public JsonResult GetClassById(int id)
        {
            Class cls = dal.GetClassById(id);
            return Json(cls);
        }


        [Obsolete]
        public ActionResult edit(int id)
        {
            Class cls = dal.GetClassById(id);
            ViewBag.ClassId = cls.Class_ID;
            return View(cls);
        }

        [HttpPost]
        [Obsolete]
        public JsonResult edit(Class cls)
        {
            dal.UpdateClass(cls);
            return Json(new { success = true, message = "Class updated successfully!" });
        }

        [HttpPost]
        [Obsolete]
        public JsonResult delete(int id)
        {
            dal.DeleteClass(id);
            return Json(new { success = true, message = "Class deleted successfully!" });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
