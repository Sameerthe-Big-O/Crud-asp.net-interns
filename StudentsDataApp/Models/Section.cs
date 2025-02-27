using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsDataApp.Models
{
    public class Section
    {
        public int Section_Id { get; set; }

        public string? Section_Name { get; set; }

        public int Class_Id { get; set; }
        public Class? Class { get; set; }
    }
}
