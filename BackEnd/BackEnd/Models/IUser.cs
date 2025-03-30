namespace BackEnd.Models
{
    public interface IUser
    {
        string Id { get; set; }

        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}
