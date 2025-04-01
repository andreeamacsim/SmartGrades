using BackEnd.Models;

namespace BackEnd.Service
{
    public interface ICourseCollectionService :ICollectionService<Course>
    {
        Task<bool> HasTeacher(string teacherId,string courseId);
        Task<bool>HasStudent(string studentId,string courseId);
        Task<List<Course>> GetTeacherCoursesList(string teacherId);

        Task<List<Student>> GetStudentsFromCourse(string courseId);
    }
}
