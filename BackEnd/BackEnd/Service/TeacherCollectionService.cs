using BackEnd.Models;
using BackEnd.Settings;
using MongoDB.Driver;

namespace BackEnd.Service
{
    public class TeacherCollectionService : ITeacherCollectionService
    {
        private readonly IMongoCollection<Teacher> _teachers;

        public TeacherCollectionService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _teachers = database.GetCollection<Teacher>(settings.TeachersCollectionName);
        }

        public async Task<bool> Create(Teacher entity)
        {
            if (entity == null)
                return false;

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();

            await _teachers.InsertOneAsync(entity);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _teachers.DeleteOneAsync(teacher => teacher.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<List<Teacher>> GetAll()
        {
            var result = await _teachers.FindAsync(_ => true);
            return await result.ToListAsync();
        }

        public async Task<Teacher?> GetById(string id)
        {
            var result = await _teachers.FindAsync(teacher => teacher.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<bool> Update(string id, Teacher entity)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            entity.Id = id;
            var result = await _teachers.ReplaceOneAsync(teacher => teacher.Id == id, entity);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}

