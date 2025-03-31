using BackEnd.Models;
using BackEnd.Settings;
using MongoDB.Driver;

namespace BackEnd.Service
{
    public class CourseCollectionService : ICourseCollectionService
    {
        private readonly IMongoCollection<Course> _courses;

        public CourseCollectionService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _courses = database.GetCollection<Course>(settings.CoursesCollectionName);
        }

        public async Task<bool> Create(Course entity)
        {
            if (entity == null)
                return false;

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();

            await _courses.InsertOneAsync(entity);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _courses.DeleteOneAsync(course => course.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<List<Course>> GetAll()
        {
            var result = await _courses.FindAsync(_ => true);
            return await result.ToListAsync();
        }

        public async Task<Course?> GetById(string id)
        {
            var result = await _courses.FindAsync(course => course.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<bool> HasStudent(string studentId,string courseId)
        {
            var results= (await _courses.FindAsync(course=>course.Id== courseId)).FirstOrDefault();
            if (results == null) return false;
            var hasStudent=results.StudentIds.FirstOrDefault(student=>student==studentId);
            if (hasStudent==null) return false;
            return true;

        }

        public async Task<bool> HasTeacher(string teacherId, string courseId)
        {
            var results = (await _courses.FindAsync(course => course.Id == courseId)).FirstOrDefault();
            if (results == null) return false;
            var hasTeacher = results.TeacherIds.FirstOrDefault(teacher => teacher == teacherId);
            if(hasTeacher==null)
                return false;
            return true;
        }

        public async Task<bool> Update(string id, Course entity)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            entity.Id = id;
            var result = await _courses.ReplaceOneAsync(course => course.Id == id, entity);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}

