using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using StudentsDataApp.Models;
using StudentsDataApp.DAL;

namespace StudentsDataApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDataAccessLayer dal;

        public StudentsController()
        {
            dal = new StudentDataAccessLayer();
        }

        [HttpGet]
        public JsonResult GetAllStudents()
        {
            List<Students> stud = dal.GetAllStudents();
            return Json(stud);
        }

      
        [HttpGet]
        public JsonResult GetStudentById(int id)
        {
            Students stud = dal.GetStudentById(id);
            if (stud == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }
            return Json(stud);
        }

        [HttpPost]
        public JsonResult AddStudent(Students stud, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageFile.CopyTo(memoryStream);
                    stud.Image = memoryStream.ToArray();
                }
            }
            else
            {
                stud.Image = null;
            }

            dal.AddStudent(stud);
            return Json(new { success = true, message = "Student added successfully" });
        }


        [HttpGet]
        public IActionResult GetStudentImage(int id)
        {
            var stud = dal.GetStudentById(id);
            if (stud == null)
            {
                return NotFound(new { success = false, message = "Student not found" });
            }

            if (stud.Image == null || stud.Image.Length == 0)
            {
                return NotFound(new { success = false, message = "Image not found" });
            }

            return File(stud.Image, "image/jpeg"); 
        }


        
        [HttpPost]
        public JsonResult UpdateStudent(Students stud, IFormFile imageFile)
        {
           
            var existingStudent = dal.GetStudentById(stud.Id);
            if (existingStudent == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }

            existingStudent.First_Name = stud.First_Name;
            existingStudent.Last_Name = stud.Last_Name;
            existingStudent.Roll_Number = stud.Roll_Number;
            existingStudent.Marks = stud.Marks;
            existingStudent.Email = stud.Email;

            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageFile.CopyTo(memoryStream);
                    existingStudent.Image = memoryStream.ToArray();
                }
            }
          

            dal.UpdateStudent(existingStudent);
            return Json(new { success = true, message = "Student updated successfully" });
        }

        [HttpDelete]
        public JsonResult DeleteStudent(int id)
        {
            var existingStudent = dal.GetStudentById(id);
            if (existingStudent == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }

            dal.DeleteStudent(id);
            return Json(new { success = true, message = "Student deleted successfully" });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
