using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.AspNetCore.Mvc;

namespace StudentsDataApp.Controllers
{
    public class ReportsController : Controller
    {
        string cs = ConnectionString.dbcs;

        public ActionResult GetAllStudents()
        {
            try
            {
                ReportDocument rd = new ReportDocument();

                string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "GetAllStudentReport.rpt");

                if (!System.IO.File.Exists(reportPath))
                {
                    return Content("Report file not found: " + reportPath);
                }

                rd.Load(reportPath);

                DataTable dt = GetStudentData("spGetAllStudent", null);
                rd.SetDataSource(dt);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                rd.Close();

                return File(stream, "application/pdf", "AllStudentsReport.pdf");
            }
            catch (Exception ex)
            {
                // Capture the detailed inner exception, if any
                string errorMessage = $"Error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Inner Exception: {ex.InnerException.Message}";
                }

                // Log the error to a file (optional)
                System.IO.File.AppendAllText("error_log.txt", $"{DateTime.Now}: {errorMessage}\n");

                return Content(errorMessage);
            }
        }


        public ActionResult GetStudentDetail(int Id)
        {
            try
            {
                ReportDocument rd = new ReportDocument();

                string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "GetStudentDetail.rpt");

                if (!System.IO.File.Exists(reportPath))
                {
                    return Content("Report file not found: " + reportPath);
                }

                rd.Load(reportPath); 

                SqlParameter[] parameters = { new SqlParameter("@Id", Id) };
                DataTable dt = GetStudentData("spGetStudentDetails", parameters);
                rd.SetDataSource(dt);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                rd.Close();
                return File(stream, "application/pdf", "StudentDetailReport.pdf");
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }

        private DataTable GetStudentData(string storedProcedure, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}