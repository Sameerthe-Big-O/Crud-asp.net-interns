namespace StudentsDataApp.Models
{
    public class Students
    {
        public int Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public int Roll_Number { get; set; }

        public int Marks { get; set; }

        public string Email { get; set; }

        public byte[] Image { get; set; }
    }
}
