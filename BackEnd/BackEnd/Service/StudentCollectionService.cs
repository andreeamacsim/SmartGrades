using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Settings;
using MongoDB.Driver;
using SharpCompress.Common;

namespace BackEnd.Service
{
    public class StudentCollectionService : IStudentCollectionService
    {
        private readonly IMongoCollection<Student> _students;

        public StudentCollectionService(IMongoDbSettings settings)
        {
            var client=new MongoClient();
            var database = client.GetDatabase(settings.ConnectionString);
            _students = database.GetCollection<Student>(settings.StudentsCollectionName);
        }
        public async Task<bool> Create(Student enity)
        {
            if (enity == null)
                return false;
            if(enity.Id==Guid.Empty.ToString())
                enity.Id=Guid.NewGuid().ToString();

            await _students.InsertOneAsync(enity);

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
    }
}
