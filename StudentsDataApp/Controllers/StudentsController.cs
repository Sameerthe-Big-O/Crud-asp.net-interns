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
        private readonly EducationHistoryDAL edudal;

        public StudentsController()
        {
            dal = new StudentDAL();
            edudal = new EducationHistoryDAL();
        }

        public IActionResult CreateEdit(int? id)
        {
            StudentsModel student = id.HasValue ? dal.GetStudentById(id.Value) : new StudentsModel();
            ViewBag.Classes = dal.GetAllClasses();
            ViewBag.EducationHistory = id.HasValue ? edudal.GetEducationHistoryByStudentId(id.Value) : new List<EducationHistoryModel>();
            return View(student);
        }

        [HttpGet]
        public JsonResult GetAllStudents()
        {
            List<StudentsModel> students = dal.GetAllStudents();
            return Json(students);
        }

        [HttpGet]
        public JsonResult GetStudentById(int id)
        {
            StudentsModel student = dal.GetStudentById(id);
            if (student == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }
            student.EducationHistory = edudal.GetEducationHistoryByStudentId(id);
            return Json(student);
        }
        [HttpPost]
        public JsonResult SaveStudent(StudentsModel student, IFormFile imageFile, List<EducationHistoryModel> educationHistory)
        {
            try
            {
                if (student.Id > 0) // Update student
                {
                    var existingStudent = dal.GetStudentById(student.Id);
                    if (existingStudent == null)
                    {
                        return Json(new { success = false, message = "Student not found" });
                    }

                    // Update student details
                    existingStudent.First_Name = student.First_Name;
                    existingStudent.Last_Name = student.Last_Name;
                    existingStudent.Roll_Number = student.Roll_Number;
                    existingStudent.Marks = student.Marks;
                    existingStudent.Email = student.Email;
                    existingStudent.ClassID = student.ClassID;
                    existingStudent.SectionID = student.SectionID;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            imageFile.CopyTo(memoryStream);
                            existingStudent.Image = memoryStream.ToArray();
                        }
                    }

                    dal.UpdateStudent(existingStudent);

                    // Handling Education History
                    var existingHistory = edudal.GetEducationHistoryByStudentId(student.Id);
                    var existingHistoryIds = existingHistory.Select(h => h.EducationHistoryId).ToList();
                    var receivedHistoryIds = educationHistory.Select(h => h.EducationHistoryId).ToList();

                    // 1️⃣ Update existing records
                    foreach (var history in educationHistory)
                    {
                        if (existingHistoryIds.Contains(history.EducationHistoryId))
                        {
                            edudal.UpdateEducationHistory(history);
                        }
                        else
                        {
                            history.Id = student.Id;
                            edudal.AddEducationHistory(history);
                        }
                    }

                    // 2️⃣ Remove deleted history records (Not in received data)
                    var historyToDelete = existingHistory.Where(h => !receivedHistoryIds.Contains(h.EducationHistoryId)).ToList();
                    foreach (var history in historyToDelete)
                    {
                        edudal.DeleteEducationHistory(history.EducationHistoryId);
                    }

                    return Json(new { success = true, message = "Student updated successfully" });
                }
                else // Add new student
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            imageFile.CopyTo(memoryStream);
                            student.Image = memoryStream.ToArray();
                        }
                    }

                    int newStudentId = dal.AddStudent(student);
                    student.Id = newStudentId; // Ensure student ID is updated

                    if (educationHistory != null && educationHistory.Count > 0)
                    {
                        foreach (var history in educationHistory)
                        {
                            history.Id = student.Id;
                            edudal.AddEducationHistory(history);
                        }
                    }

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

            var student = dal.GetStudentById(id);
            if (student == null || student.Image == null || student.Image.Length == 0)
            {
                return NotFound(new { success = false, message = "Image not found" });
            }

            return File(student.Image, "image/jpeg");
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
            if (classId <= 0)
            {
                return Json(new { success = false, message = "Invalid Class ID" });
            }

            var sections = dal.GetSectionsByClassId(classId);
            if (sections == null || sections.Count == 0)
            {
                return Json(new { success = false, message = "No sections found for this class." });
            }
            return Json(sections);
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
