using Microsoft.AspNetCore.Mvc;
using StudentsDataApp.DAL;
using StudentsDataApp.Models;

namespace StudentsDataApp.Controllers
{
    public class SectionController : Controller
    {

        private readonly SectionDAL dal;

        public SectionController()
        {
            dal = new SectionDAL();
        }

        [Obsolete]
        public JsonResult GetAllSections()
        {
            List<SectionModel> sectionList = dal.GetAllSections();
            return Json(sectionList);
        }
        public ActionResult CreateEdit(int? id)
        {
            ViewBag.ClassList = dal.GetAllClasses();

            SectionModel sec = new SectionModel();

            if (id.HasValue) 
            {
                sec = dal.GetSectionById(id.Value);
            }

            return View(sec);
        }
        [HttpPost]
        public JsonResult SaveSection(SectionModel section)
        {
            string message = "";

            if (section.SectionId > 0)
            {
              
                message = dal.UpdateSection(section);
            }
            else
            {
                
                message = dal.AddSection(section);
            }

          
            bool success = message.Contains("successfully");

            return Json(new { success = success, message = message });
        }

        [HttpPost]
        [Obsolete]
        public JsonResult DeleteSection(int sectionId)
        {
            string message = dal.DeleteSection(sectionId);
            bool isDeleted = message.Contains("successfully");

            return Json(new { success = isDeleted, message = message });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
