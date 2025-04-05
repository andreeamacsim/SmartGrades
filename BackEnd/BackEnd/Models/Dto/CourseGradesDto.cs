namespace BackEnd.Models.Dto
{
    public class CourseGradesDto
    {
        public string CourseName { get; set; }
        public List<Grade> Grades { get; set; }
        public decimal CourseAverage { get; set; }
    }
}
