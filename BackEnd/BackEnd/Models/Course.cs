using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEnd.Models
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> TeacherIds { get; set; } = new List<string>();

        public List<string> StudentIds { get; set; } = new List<string>(); 
    }
}
