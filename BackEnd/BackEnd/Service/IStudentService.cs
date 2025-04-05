using BackEnd.Models;
using BackEnd.Models.Dto;

namespace BackEnd.Service
{
    public interface IStudentCollectionService : ICollectionService<Student>
    {
        Task<Student> VerifyAccount(string username, string password);
        Task<List<Grade>> GetGrades(string id);
        Task<bool> AddGrade(string userId, Grade grade);
        Task<DashboardSummaryDto> GetStudentDashboard(string studentId);
    }
}
