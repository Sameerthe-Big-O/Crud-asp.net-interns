using StudentsDataApp.Models;
using System.Data.SqlClient;
using System.Data;

namespace StudentsDataApp.DAL
{
    public class ClassDAL
    {
        string cs = ConnectionString.dbcs;


        [Obsolete]
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

        [Obsolete]
        public ClassModel GetClassById(int classID)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetClassByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassID", classID);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    return new ClassModel { ClassID = (int)rdr["ClassID"], ClassName = rdr["ClassName"].ToString() };
                }
                return null;
            }
        }

        public string AddClass(ClassModel cls)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spAddClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassName", cls.ClassName);

                con.Open();
                string message = cmd.ExecuteScalar()?.ToString(); 
                con.Close();

                return message ?? "Unknown error occurred.";
            }
        }

        public string UpdateClass(ClassModel cls)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassID", cls.ClassID);
                cmd.Parameters.AddWithValue("@ClassName", cls.ClassName);

                con.Open();
                string message = cmd.ExecuteScalar()?.ToString();
                con.Close();

                return message ?? "Unknown error occurred.";
            }
        }

        [Obsolete]
        public string DeleteClass(int classID)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spDeleteClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassID", classID);

                con.Open();
                string message = cmd.ExecuteScalar()?.ToString();
                con.Close();

                return message ?? "Unknown error occurred.";
            }
        }
    }
}
