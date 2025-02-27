using Microsoft.AspNetCore.Mvc;
using StudentsDataApp.DAL;
using StudentsDataApp.Models;

namespace StudentsDataApp.Controllers
{
    public class SectionController : Controller
    {
        private readonly SectionDataAccessLayer dal;

        public SectionController()
        {
            dal = new SectionDataAccessLayer();
        }

        [Obsolete]
        public JsonResult GetAllSections()
        {
            List<Section> sectionlist = dal.GetAllSections();
            return Json(sectionlist);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Obsolete]
        public JsonResult Create(Section sec)
        { 
            dal.AddSection(sec);
            return Json(new { success = true, message = "Section added successfully!" });
        }

        [Obsolete]
        public JsonResult GetAllClasses()
        {
            List<Class> clslist = dal.GetAllClasses();
            return Json(clslist);

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
