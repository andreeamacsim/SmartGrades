using BackEnd.Models;
using BackEnd.Settings;
using MongoDB.Driver;

namespace BackEnd.Service
{
    public class GradeCollectionService : IGradeCollectionService
    {
        private readonly IMongoCollection<Grade> _grades;

        public GradeCollectionService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _grades = database.GetCollection<Grade>(settings.GradesCollectionName);
        }

        public async Task<bool> Create(Grade entity)
        {
            if (entity == null)
                return false;

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();

            await _grades.InsertOneAsync(entity);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _grades.DeleteOneAsync(grade => grade.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<List<Grade>> GetAll()
        {
            var result = await _grades.FindAsync(_ => true);
            return await result.ToListAsync();
        }

        public async Task<Grade?> GetById(string id)
        {
            var result = await _grades.FindAsync(grade => grade.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<bool> Update(string id, Grade entity)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            entity.Id = id;
            var result = await _grades.ReplaceOneAsync(grade => grade.Id == id, entity);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
