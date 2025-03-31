using BackEnd.Models;
using BackEnd.Settings;
using MongoDB.Driver;

namespace BackEnd.Service
{
    public class TeacherCollectionService : ITeacherCollectionService
    {
        private readonly IMongoCollection<Teacher> _teachers;
        private readonly IStudentCollectionService _students;
        private readonly ICourseCollectionService _courses;
        public TeacherCollectionService(IMongoDbSettings settings ,IStudentCollectionService studentCollectionService,ICourseCollectionService courseCollectionService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _teachers = database.GetCollection<Teacher>(settings.TeachersCollectionName);
            _students = studentCollectionService;
            _courses = courseCollectionService;
        }

        public async Task<bool> AddGrade(Grade grade)
        {
            if( !await _courses.HasTeacher(grade.TeacherId,grade.CourseId)&& !await _courses.HasStudent(grade.StudentId,grade.CourseId))
            {
                return false;
            }
            if(string.IsNullOrEmpty(grade.Id))
                grade.Id = Guid.NewGuid().ToString();
            var results=await _students.AddGrade(grade.StudentId, grade);
            if (results)
                return true;
            return false;
        }

        public async Task<bool> Create(Teacher entity)
        {
            if (entity == null)
                return false;

            var existingTeacher = await _teachers.Find(t => t.Username == entity.Username || t.Email == entity.Email).FirstOrDefaultAsync();

            if (existingTeacher != null)
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
        public async Task<Teacher> VerifyAccount(string username, string password)
        {
            var teacher = (await _teachers.FindAsync(teacher => teacher.Username == username && teacher.Password == password)).FirstOrDefault();
            if (teacher == null)
                return null;
            return teacher;
        }
    }
}

