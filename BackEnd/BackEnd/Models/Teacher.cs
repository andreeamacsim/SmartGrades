namespace BackEnd.Models
{
    public class Teacher : IUser
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? ResetPasswordToken { get; set; }

        public DateTime ResetPasswordExpiry { get; set; }

        public List<string> CourseIds { get; set; } = new List<string>();
        public string? Token { get; set ; }
    }
}
