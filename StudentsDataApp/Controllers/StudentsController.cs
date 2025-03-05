using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using StudentsDataApp.Models;
using StudentsDataApp.DAL;
using System.Diagnostics;

namespace StudentsDataApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDAL dal;

        public StudentsController()
        {
            dal = new StudentDAL();
        }

        public IActionResult CreateEdit(int? id)
        {
            StudentsModel student = id.HasValue ? dal.GetStudentById(id.Value) : new StudentsModel();
            ViewBag.Classes = dal.GetAllClasses();  
            return View(student);
        }
        [HttpGet]
        public JsonResult GetAllStudents()
        {
            List<StudentsModel> stud = dal.GetAllStudents();
            return Json(stud);
        }

      
        [HttpGet]
        public JsonResult GetStudentById(int id)
        {
            StudentsModel stud = dal.GetStudentById(id);
            if (stud == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }
            return Json(stud);
        }
        [HttpPost]
        public JsonResult SaveStudent(StudentsModel stud, IFormFile imageFile)
        {
            try
            {
                if (stud.Id > 0)
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
                    existingStudent.ClassID = stud.ClassID;
                    existingStudent.ClassName = stud.ClassName;
                    existingStudent.SectionID = stud.SectionID;
                    existingStudent.SectionName = stud.SectionName;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            imageFile.CopyTo(memoryStream);
                            existingStudent.Image = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        existingStudent.Image = dal.GetStudentById(stud.Id).Image; 
                    }

                    dal.UpdateStudent(existingStudent);
                    return Json(new { success = true, message = "Student updated successfully" });
                }
                else
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
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }





        [HttpGet]
        public IActionResult GetStudentImage(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid student ID" });
            }

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
        [HttpGet]
        
        public JsonResult GetSectionsByClassId(int classId)
        {
            Debug.WriteLine($"Received Class ID: {classId}");

            if (classId <= 0)
            {
                return Json(new { success = false, message = "Invalid Class ID" });
            }

            var section = dal.GetSectionsByClassId(classId);
            Debug.WriteLine($"Sections Count: {section.Count}");

            if (section == null || section.Count == 0)
            {
                return Json(new { success = false, message = "No sections found for this class." });
            }

            return Json(section);
        }




        public JsonResult GetAllClasses()
        {
            List<ClassModel> classList = dal.GetAllClasses();
            return Json(classList);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
