using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEnd.Models
{
    public interface IUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string? Token { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpiry { get; set; }
    }
}
