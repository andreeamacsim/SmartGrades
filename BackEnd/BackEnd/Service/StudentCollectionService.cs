using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Settings;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using SharpCompress.Common;

namespace BackEnd.Service
{
    public class StudentCollectionService : IStudentCollectionService
    {
        private readonly IMongoCollection<Student> _students;

        public StudentCollectionService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _students = database.GetCollection<Student>(settings.StudentsCollectionName);
        }
        public async Task<bool> Create(Student entity)
        {
            if (entity == null)
                return false;

            var existingStudent = await _students.Find(s => s.Username == entity.Username || s.Email == entity.Email).FirstOrDefaultAsync();

            if (existingStudent != null)
                return false;

            if (entity.Id == Guid.Empty.ToString())
                entity.Id = Guid.NewGuid().ToString();

            await _students.InsertOneAsync(entity);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var results = await _students.DeleteOneAsync(student => student.Id.ToString() == id);
            if (!results.IsAcknowledged && results.DeletedCount == 0)
                return false;
            return true;
        }

        public async Task<List<Student>> GetAll()
        {
            var result = await _students.FindAsync(task => true);
            return result.ToList();
        }

        public async Task<Student> GetById(string id)
        {
            return (await _students.FindAsync(student => student.Id.ToString() == id)).FirstOrDefault();
        }

        public async Task<bool> Update(string id, Student entity)
        {
            if (id == null)
                return false;
            entity.Id = id;
            var results = await _students.ReplaceOneAsync(student => student.Id.ToString() == id, entity);
            if (!results.IsAcknowledged && results.ModifiedCount == 0)
            {
                await _students.InsertOneAsync(entity);
                return false;
            }
            return true;
        }

        public async Task<Student> VerifyAccount(string username, string password)
        {
            var student = (await _students.FindAsync(stundent => stundent.Username == username && stundent.Password == password)).FirstOrDefault();
            if (student == null)
                return null;
            return student;
        }
    }
}
