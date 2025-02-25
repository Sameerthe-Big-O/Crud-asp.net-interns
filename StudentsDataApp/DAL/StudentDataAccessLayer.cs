using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using StudentsDataApp.Models;

namespace StudentsDataApp.DAL
{
    public class StudentDataAccessLayer
    {
        string cs = ConnectionString.dbcs;

        public List<Students> GetAllStudents()
        {
            List<Students> studList = new List<Students>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Students stud = new Students
                    {
                        Id = reader["Id"] as int? ?? 0,
                        First_Name = reader["First_Name"] as string ?? string.Empty,
                        Last_Name = reader["Last_Name"] as string ?? string.Empty,
                        Roll_Number = reader["Roll_Number"] as int? ?? 0,
                        Marks = reader["Marks"] as int? ?? 0,
                        Email = reader["Email"] as string ?? string.Empty,
                        Image = reader["Image"] as byte[] ?? null
                    };


                    studList.Add(stud);
                }
            }
            return studList;
        }

        public Students GetStudentById(int id)
        {
            Students stud = null;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Students WHERE Id = @id", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    stud = new Students
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        First_Name = reader["First_Name"].ToString(),
                        Last_Name = reader["Last_Name"].ToString(),
                        Roll_Number = Convert.ToInt32(reader["Roll_Number"]),
                        Marks = Convert.ToInt32(reader["Marks"]),
                        Email = reader["Email"].ToString(),
                        Image = reader["Image"] as byte[] ?? null
                    };
                }
            }
            return stud;
        }


        public void AddStudent(Students stud)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spAddStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@first_Name", stud.First_Name);
                cmd.Parameters.AddWithValue("@last_Name", stud.Last_Name);
                cmd.Parameters.AddWithValue("@roll_Number", stud.Roll_Number);
                cmd.Parameters.AddWithValue("@marks", stud.Marks);
                cmd.Parameters.AddWithValue("@email", stud.Email);
                cmd.Parameters.Add("@image", SqlDbType.VarBinary, -1).Value = (object)stud.Image ?? DBNull.Value;

                con.Open();
                cmd.ExecuteNonQuery();
                
            }
        }


        public void UpdateStudent(Students stud)
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


                con.Open();
                cmd.ExecuteNonQuery();
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
    }
}
