using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using StudentsDataApp.Models;

namespace StudentsDataApp.DAL
{
    public class ClassDataAccessLayer
    {
        string cs = ConnectionString.dbcs;

        [Obsolete]
        public List<Class> GetAllClasses()
        {
            List<Class> classList = new List<Class>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllClasses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Class cls = new Class
                    {
                        Class_ID = Convert.ToInt32(reader["class_id"]),
                        Class_Name = reader["class_name"].ToString()
                    };
                    classList.Add(cls);
                }
            }
            return classList;
        }


        [Obsolete]
        public Class GetClassById(int id)
        {
            Class cls = null;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetClassById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("class_id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    cls = new Class
                    {
                        Class_ID = Convert.ToInt32(reader["class_id"]),
                        Class_Name = reader["class_name"].ToString()
                    };
                }
            }
            return cls;
        }

        [Obsolete]
        public bool ClassExists(Class cls)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Class WHERE Class_Name = @class_name", con);
                cmd.Parameters.AddWithValue("@class_name", cls.Class_Name);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }


        [Obsolete]
        public bool AddClass(Class cls)
        {
            using(SqlConnection con = new SqlConnection(cs))
            {
                if (ClassExists(cls))
                {
                    return false; 
                }
                SqlCommand cmd = new SqlCommand("spAddClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@class_name", cls.Class_Name ?? (object)DBNull.Value);


                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        [Obsolete]
        public void UpdateClass(Class cls)
        {
            using(SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@class_id", cls.Class_ID);
                cmd.Parameters.AddWithValue("@class_name", cls.Class_Name ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [Obsolete]
        public void DeleteClass(int id)
        {
            using(SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spDeleteClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@class_id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
