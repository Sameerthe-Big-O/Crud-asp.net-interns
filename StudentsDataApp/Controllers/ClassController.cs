using Microsoft.AspNetCore.Mvc;
using StudentsDataApp.DAL;
using StudentsDataApp.Models;

namespace StudentsDataApp.Controllers
{
    public class ClassController : Controller
    {

        private readonly ClassDAL dal;

        public ClassController()
        {
            dal = new ClassDAL();
        }


        [Obsolete]
        public JsonResult GetAllClasses()
        {
            List<ClassModel> classList = dal.GetAllClasses();
            return Json(classList);
        }



        [HttpGet]
        public IActionResult CreateEdit(int? id)
        {
            if (id == null)
            {
                return View(new ClassModel()); 
            }

            ClassModel cls = dal.GetClassById(id.Value);
            return View(cls);
        }

        [HttpPost]
        public JsonResult SaveClass(ClassModel cls)
        {
            if (cls == null || string.IsNullOrWhiteSpace(cls.ClassName))
            {
                return Json(new { success = false, message = "Class name is required." });
            }

            try
            {
                string resultMessage = cls.ClassID == 0 ? dal.AddClass(cls) : dal.UpdateClass(cls);

                if (resultMessage.Contains("exists"))
                {
                    return Json(new { success = false, message = resultMessage });
                }

                return Json(new { success = true, message = resultMessage });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }





        [HttpPost]
        [Obsolete]
        public JsonResult DeleteClass(int classID)
        {
            bool isDeleted = dal.DeleteClass(classID);
            return Json(new { success = isDeleted, message = isDeleted ? "Class deleted successfully!" : "Error deleting class!" });
        }


       
        public IActionResult Index()
        {
            return View();
        }
    }
}
