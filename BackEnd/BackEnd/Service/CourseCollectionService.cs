using BackEnd.Models;
using BackEnd.Settings;
using MongoDB.Driver;

namespace BackEnd.Service
{
    /// <summary>
    /// Service for managing basic operations on the course collection in MongoDB.
    /// Allows creating, updating, deleting, retrieving courses, and managing students and teachers associated with courses.
    /// </summary>
    public class CourseCollectionService : ICourseCollectionService
    {
        private readonly IMongoCollection<Course> _courses;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseCollectionService"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings containing the connection string, database name, and collection name.</param>
        public CourseCollectionService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _courses = database.GetCollection<Course>(settings.CoursesCollectionName);
        }

        /// <summary>
        /// Creates a new course record in the database.
        /// </summary>
        /// <param name="entity">The course entity to be inserted.</param>
        /// <returns>True if the operation was successful, otherwise false.</returns>
        public async Task<bool> Create(Course entity)
        {
            if (entity == null)
                return false;

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();

            await _courses.InsertOneAsync(entity);
            return true;
        }

        /// <summary>
        /// Deletes a course record from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the course record to delete.</param>
        /// <returns>True if the course was deleted successfully, otherwise false.</returns>
        public async Task<bool> Delete(string id)
        {
            var result = await _courses.DeleteOneAsync(course => course.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        /// <summary>
        /// Retrieves all course records from the database.
        /// </summary>
        /// <returns>A list of all course records.</returns>
        public async Task<List<Course>> GetAll()
        {
            var result = await _courses.FindAsync(_ => true);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Retrieves a course record by its ID.
        /// </summary>
        /// <param name="id">The ID of the course record to retrieve.</param>
        /// <returns>The course record if found, otherwise null.</returns>
        public async Task<Course?> GetById(string id)
        {
            var result = await _courses.FindAsync(course => course.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a list of students associated with a specific course.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A list of students enrolled in the course, or null if the course does not exist.</returns>
        public async Task<List<Student>> GetStudentsFromCourse(string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
                return null;

            var course = (await _courses.FindAsync(course => course.Id == courseId)).FirstOrDefault();
            if (course == null)
                return null;

            return course.Students;
        }

        /// <summary>
        /// Retrieves a list of courses taught by a specific teacher.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher.</param>
        /// <returns>A list of courses taught by the teacher.</returns>
        public async Task<List<Course>> GetTeacherCoursesList(string teacherId)
        {
            var cursor = await _courses.FindAsync(course => course.Teachers.Any(teacher => teacher.Id == teacherId));
            var results = await cursor.ToListAsync();
            return results;
        }

        /// <summary>
        /// Checks if a student is enrolled in a specific course.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the student is enrolled in the course, otherwise false.</returns>
        public async Task<bool> HasStudent(string studentId, string courseId)
        {
            var course = (await _courses.FindAsync(course => course.Id == courseId)).FirstOrDefault();
            if (course == null) return false;

            var hasStudent = course.Students.FirstOrDefault(student => student.Id == studentId);
            return hasStudent != null;
        }

        /// <summary>
        /// Checks if a teacher is assigned to a specific course.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher.</param>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>True if the teacher is assigned to the course, otherwise false.</returns>
        public async Task<bool> HasTeacher(string teacherId, string courseId)
        {
            var course = (await _courses.FindAsync(course => course.Id == courseId)).FirstOrDefault();
            if (course == null) return false;

            var hasTeacher = course.Teachers.FirstOrDefault(teacher => teacher.Id == teacherId);
            return hasTeacher != null;
        }

        /// <summary>
        /// Updates an existing course record by its ID.
        /// </summary>
        /// <param name="id">The ID of the course record to update.</param>
        /// <param name="entity">The course entity containing updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
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

