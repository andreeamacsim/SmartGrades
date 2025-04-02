namespace BackEnd.Models
{
    public class Student : IUser
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? ResetPasswordToken { get; set; }

        public DateTime ResetPasswordExpiry { get; set; }

        public List<string> CourseIds { get; set; } = new List<string>();

        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}
