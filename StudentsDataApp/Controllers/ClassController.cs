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
        public ActionResult GetAllClasses()
        {
            List<Class> cls = dal.GetAllClasses();
            return View(cls);
            
        }

       
        [Obsolete]
        public JsonResult create(Class cls)
        {
            dal.AddClass(cls);
            return Json(new { success = true, message = "Class added successfully!" });
        }

        [Obsolete]
        public ActionResult edit(int id)
        {
            Class cls = dal.GetClassById(id);
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
