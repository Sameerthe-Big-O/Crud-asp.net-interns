using StudentsDataApp.Models;
using System.Data.SqlClient;
using System.Data;

namespace StudentsDataApp.DAL
{
    public class EducationHistoryDAL
    {
        string cs = ConnectionString.dbcs;

        public List<EducationHistoryModel> GetEducationHistoryByStudentId(int Id)
        {
            List<EducationHistoryModel> historyList = new List<EducationHistoryModel>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetEducationHistoryByStudentId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    EducationHistoryModel history = new EducationHistoryModel
                    {
                        EducationHistoryId = Convert.ToInt32(reader["EducationHistoryId"]),
                        Id = Convert.ToInt32(reader["Id"]),
                        PreviousSchool = reader["PreviousSchool"].ToString(),
                        PreviousClass = reader["PreviousClass"].ToString()
                    };
                    historyList.Add(history);
                }
            }
            return historyList;
        }

        public void AddEducationHistory(EducationHistoryModel history)
        {
            if (string.IsNullOrEmpty(history.PreviousSchool) || string.IsNullOrEmpty(history.PreviousClass))
            {
                return; 
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spAddEducationHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", history.Id);
                cmd.Parameters.AddWithValue("@PreviousSchool", history.PreviousSchool);
                cmd.Parameters.AddWithValue("@PreviousClass", history.PreviousClass);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateEducationHistory(EducationHistoryModel history)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateEducationHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EducationHistoryId", history.EducationHistoryId);
                cmd.Parameters.AddWithValue("@Id", history.Id);
                cmd.Parameters.AddWithValue("@PreviousSchool", history.PreviousSchool);
                cmd.Parameters.AddWithValue("@PreviousClass", history.PreviousClass);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                Console.WriteLine($"Updated Rows: {rowsAffected} for History ID: {history.EducationHistoryId}");
            }
        }


        public void DeleteEducationHistory(int educationHistoryId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DeleteEducationHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EducationHistoryId", educationHistoryId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
