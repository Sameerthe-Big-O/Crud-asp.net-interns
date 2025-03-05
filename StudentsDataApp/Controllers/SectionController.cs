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
            bool result = false;
            string message = "";

            
            if (dal.SectionExists(section.SectionName, section.ClassId))
            {
                return Json(new { success = false, message = "This section already exists in the selected class. Try a different section." });
            }

            if (section.SectionId > 0)
            {
                result = dal.UpdateSection(section);
                message = result ? "Section updated successfully!" : "Failed to update section.";
            }
            else
            {
                result = dal.AddSection(section);
                message = result ? "Section added successfully!" : "Failed to add section.";
            }

            return Json(new { success = result, message = message });
        }




        [HttpPost]
        [Obsolete]
        public JsonResult DeleteSection(int sectionId)
        {
            bool isDeleted = dal.DeleteSection(sectionId);
            return Json(new { success = isDeleted, message = isDeleted ? "Section deleted successfully!" : "Error deleting section!" });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
