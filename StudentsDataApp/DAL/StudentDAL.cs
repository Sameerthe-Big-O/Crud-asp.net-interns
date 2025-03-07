using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudentsDataApp.Models;

namespace StudentsDataApp.DAL
{
    public class StudentDAL
    {
        string cs = ConnectionString.dbcs;
        private EducationHistoryDAL edudal = new EducationHistoryDAL();

        public List<StudentsModel> GetAllStudents()
        {
            List<StudentsModel> studList = new List<StudentsModel>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    StudentsModel stud = new StudentsModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        First_Name = reader["First_Name"].ToString(),
                        Last_Name = reader["Last_Name"].ToString(),
                        Roll_Number = Convert.ToInt32(reader["Roll_Number"]),
                        Marks = Convert.ToInt32(reader["Marks"]),
                        Email = reader["Email"].ToString(),
                        Image = reader["Image"] as byte[] ?? null,
                        ClassID = Convert.ToInt32(reader["ClassID"]),
                        ClassName = reader["ClassName"].ToString(),
                        SectionID = Convert.ToInt32(reader["SectionID"]),
                        SectionName = reader["SectionName"].ToString(),
                        EducationHistory = edudal.GetEducationHistoryByStudentId(Convert.ToInt32(reader["Id"]))
                    };

                    studList.Add(stud);
                }
            }
            return studList;
        }

        public StudentsModel GetStudentById(int id)
        {
            StudentsModel stud = null;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetStudentById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    stud = new StudentsModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        First_Name = reader["First_Name"].ToString(),
                        Last_Name = reader["Last_Name"].ToString(),
                        Roll_Number = Convert.ToInt32(reader["Roll_Number"]),
                        Marks = Convert.ToInt32(reader["Marks"]),
                        Email = reader["Email"].ToString(),
                        Image = reader["Image"] as byte[] ?? null,
                        ClassID = Convert.ToInt32(reader["ClassID"]),
                        ClassName = reader["ClassName"].ToString(),
                        SectionID = Convert.ToInt32(reader["SectionID"]),
                        SectionName = reader["SectionName"].ToString(),
                        EducationHistory = edudal.GetEducationHistoryByStudentId(id)
                    };
                }
            }
            return stud;
        }

        public int AddStudent(StudentsModel stud)
        {
            int newStudentId = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spAddStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@First_Name", stud.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", stud.Last_Name);
                cmd.Parameters.AddWithValue("@Roll_Number", stud.Roll_Number);
                cmd.Parameters.AddWithValue("@Marks", stud.Marks);
                cmd.Parameters.AddWithValue("@Email", stud.Email);
                cmd.Parameters.AddWithValue("@ClassID", stud.ClassID);
                cmd.Parameters.AddWithValue("@SectionID", stud.SectionID);
                cmd.Parameters.Add("@image", SqlDbType.VarBinary, -1).Value = (object)stud.Image ?? DBNull.Value;

                SqlParameter outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                con.Open();
                cmd.ExecuteNonQuery();
                newStudentId = Convert.ToInt32(outputIdParam.Value);
            }

            return newStudentId;
        }


        public void UpdateStudent(StudentsModel stud)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", stud.Id);
                cmd.Parameters.AddWithValue("@first_Name", stud.First_Name);
                cmd.Parameters.AddWithValue("@last_Name", stud.Last_Name);
                cmd.Parameters.AddWithValue("@roll_Number", stud.Roll_Number);
                cmd.Parameters.AddWithValue("@marks", stud.Marks);
                cmd.Parameters.AddWithValue("@email", stud.Email);
                cmd.Parameters.Add("@image", SqlDbType.VarBinary, -1).Value = (object)stud.Image ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@class_id", stud.ClassID);
                cmd.Parameters.AddWithValue("@section_id", stud.SectionID);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            if (stud.EducationHistory != null && stud.EducationHistory.Count > 0)
            {
                foreach (var history in stud.EducationHistory)
                {
                    if (history.EducationHistoryId > 0)
                    {
                        // Update only if ID exists
                        edudal.UpdateEducationHistory(history);
                    }
                    else
                    {
                        // Check if record exists before adding
                        var existingRecords = edudal.GetEducationHistoryByStudentId(stud.Id);
                        bool exists = existingRecords.Any(h => h.PreviousSchool == history.PreviousSchool && h.PreviousClass == history.PreviousClass);

                        if (!exists)  // Only insert if it does not exist
                        {
                            history.Id = stud.Id;
                            edudal.AddEducationHistory(history);
                        }
                    }
                }
            }
        }


        public void DeleteStudent(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spDeleteStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<ClassModel> GetAllClasses()
        {
            List<ClassModel> classList = new List<ClassModel>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllClasses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ClassModel cls = new ClassModel
                    {
                        ClassID = Convert.ToInt32(reader["ClassID"]),
                        ClassName = reader["ClassName"].ToString()
                    };
                    classList.Add(cls);
                }
            }
            return classList;
        }

        public List<SectionModel> GetSectionsByClassId(int classId)
        {
            List<SectionModel> sectionList = new List<SectionModel>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetSectionsByClassId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", classId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SectionModel section = new SectionModel
                    {
                        SectionId = Convert.ToInt32(reader["SectionId"]),
                        SectionName = reader["SectionName"].ToString(),
                        ClassId = Convert.ToInt32(reader["ClassId"])
                    };
                    sectionList.Add(section);
                }
            }
            return sectionList;
        }
    }
}
