using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IStudentCollectionService : ICollectionService<Student>
    {
        Task<Student> VerifyAccount(string username, string password);

    }
}
