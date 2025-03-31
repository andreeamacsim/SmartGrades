using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEnd.Models
{
    public class Course
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<Teacher> Teachers { get; set; } = new List<Teacher>();

        public List<Student> Students { get; set; } = new List<Student>(); 
    }
}
