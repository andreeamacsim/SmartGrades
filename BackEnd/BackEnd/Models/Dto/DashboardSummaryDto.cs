namespace BackEnd.Models.Dto
{
    public class DashboardSummaryDto
    {
        public List<CourseGradesDto> CourseGrades { get; set; }
        public decimal OverallGPA { get; set; }
    }
}
