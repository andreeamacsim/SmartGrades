using BackEnd.Models;
using BackEnd.Settings;
using MongoDB.Driver;

namespace BackEnd.Service
{
    /// <summary>
    /// Service for managing basic operations on the grade collection in MongoDB.
    /// Allows creating, updating, deleting, and retrieving grades.
    /// </summary>
    public class GradeCollectionService : IGradeCollectionService
    {
        private readonly IMongoCollection<Grade> _grades;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeCollectionService"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings containing the connection string, database name, and collection name.</param>
        public GradeCollectionService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _grades = database.GetCollection<Grade>(settings.GradesCollectionName);
        }

        /// <summary>
        /// Creates a new grade record in the database.
        /// </summary>
        /// <param name="entity">The grade entity to be inserted.</param>
        /// <returns>True if the operation was successful, otherwise false.</returns>
        public async Task<bool> Create(Grade entity)
        {
            if (entity == null)
                return false;

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();

            await _grades.InsertOneAsync(entity);
            return true;
        }

        /// <summary>
        /// Deletes a grade record from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the grade record to delete.</param>
        /// <returns>True if the grade was deleted successfully, otherwise false.</returns>
        public async Task<bool> Delete(string id)
        {
            var result = await _grades.DeleteOneAsync(grade => grade.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        /// <summary>
        /// Retrieves all grade records from the database.
        /// </summary>
        /// <returns>A list of all grade records.</returns>
        public async Task<List<Grade>> GetAll()
        {
            var result = await _grades.FindAsync(_ => true);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Retrieves a grade record by its ID.
        /// </summary>
        /// <param name="id">The ID of the grade record to retrieve.</param>
        /// <returns>The grade record if found, otherwise null.</returns>
        public async Task<Grade?> GetById(string id)
        {
            var result = await _grades.FindAsync(grade => grade.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates an existing grade record by its ID.
        /// </summary>
        /// <param name="id">The ID of the grade record to update.</param>
        /// <param name="entity">The grade entity containing updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
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
