using BackEnd.Models;

namespace BackEnd.Service
{
    public interface ITeacherCollectionService : ICollectionService<Teacher>
    {
        Task<Teacher> VerifyAccount(string username, string password);
        Task<bool> AddGrade(Grade grade);

    }
}
