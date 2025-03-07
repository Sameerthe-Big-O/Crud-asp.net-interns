namespace StudentsDataApp.Models
{
    public class StudentsModel
    {
        public int Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public int Roll_Number { get; set; }

        public int Marks { get; set; }

        public string Email { get; set; }

        public byte[] Image { get; set; }

        public int ClassID { get; set; }

        public string ClassName { get; set; }

        public int SectionID { get; set; }

        public string SectionName { get; set; }

        public List<EducationHistoryModel> EducationHistory { get; set; }
    }
}
