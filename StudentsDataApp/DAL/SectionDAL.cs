using StudentsDataApp.Models;
using System.Data.SqlClient;
using System.Data;

namespace StudentsDataApp.DAL
{
    public class SectionDAL
    {
        string cs = ConnectionString.dbcs;

       
        [Obsolete]
        public List<SectionModel> GetAllSections()
        {
            List<SectionModel> sectionlist = new List<SectionModel>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("spGetAllSections", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            SectionModel sec = new SectionModel
                            {
                                SectionId = Convert.ToInt32(dr["SectionId"]),
                                SectionName = dr["SectionName"].ToString(),
                                ClassId = Convert.ToInt32(dr["ClassId"]),
                                ClassName = dr["ClassName"].ToString()
                            };
                            sectionlist.Add(sec);
                        }
                    }
                }
            }
            return sectionlist;
        }
   
        [Obsolete]
        public SectionModel GetSectionById(int sectionId)
        {
            SectionModel section = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("spGetSectionById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", sectionId);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            section = new SectionModel
                            {
                                SectionId = Convert.ToInt32(dr["SectionId"]),
                                SectionName = dr["SectionName"].ToString(),
                                ClassId = Convert.ToInt32(dr["ClassId"])
                            };
                        }
                    }
                }
            }
            return section;
        }
        public string AddSection(SectionModel sec)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spAddSection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SectionName", sec.SectionName);
                cmd.Parameters.AddWithValue("@ClassId", sec.ClassId);

                con.Open();
                string message = cmd.ExecuteScalar()?.ToString();
                con.Close();

                return message ?? "Unknown error occurred.";
            }
        }

        public string UpdateSection(SectionModel sec)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateSection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SectionId", sec.SectionId);
                cmd.Parameters.AddWithValue("@SectionName", sec.SectionName);
                cmd.Parameters.AddWithValue("@ClassId", sec.ClassId);

                con.Open();
                string message = cmd.ExecuteScalar()?.ToString();
                con.Close();

                return message ?? "Unknown error occurred.";
            }
        }

        [Obsolete]
        public string DeleteSection(int sectionId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spDeleteSection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SectionId", sectionId);

                con.Open();
                string message = cmd.ExecuteScalar()?.ToString();
                con.Close();

                return message ?? "Unknown error occurred.";
            }
        }

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
    }
}
