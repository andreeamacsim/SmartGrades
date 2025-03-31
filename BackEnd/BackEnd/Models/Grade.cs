using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEnd.Models
{
    public class Grade
    {
        public string Id { get; set; }

        public string CourseId { get; set; }
        public string TeacherId { get; set; }
        public string StudentId { get; set; }

        public string AssignmentName { get; set; }

        public decimal Score { get; set; }

        public decimal MaxGrade { get; set; }

        public DateTime GradedDate { get; set; }
    }
}
