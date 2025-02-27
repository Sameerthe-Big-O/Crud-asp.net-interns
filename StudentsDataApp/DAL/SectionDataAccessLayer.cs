using System.Data;
using System.Data.SqlClient;
using StudentsDataApp.Models;

namespace StudentsDataApp.DAL
{
    public class SectionDataAccessLayer
    {
        string cs = ConnectionString.dbcs;

        [Obsolete]
        public List<Section> GetAllSections()
        {
            List<Section> sectionlist = new List<Section>();


            using(SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("GetAllSections", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Section sec = new Section
                    {
                        Section_Id = Convert.ToInt32(reader["section_id"]),
                        Section_Name = reader["section_name"].ToString(),
                        Class_Id = Convert.ToInt32(reader["class_id"]),
                        Class = new Class
                        {
                            Class_ID = Convert.ToInt32(reader["class_id"]),
                            Class_Name = reader["class_name"].ToString()
                        }
                    };
                    sectionlist.Add(sec);
                }
            }

            return sectionlist;
        }

        [Obsolete]
        public void AddSection(Section sec)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                
                SqlCommand cmd = new SqlCommand("AddSection", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@section_name", sec.Section_Name);
                cmd.Parameters.AddWithValue("@class_id", sec.Class_Id);
                con.Open();
                cmd.ExecuteNonQuery();
               
            }
        }

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
    }
}
