using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Models.Dto;
using BackEnd.Settings;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using SharpCompress.Common;

namespace BackEnd.Service
{
    /// <summary>
    /// Service for managing basic operations on the student collection in MongoDB.
    /// Allows creating, updating, deleting, retrieving students, managing grades, and verifying student accounts.
    /// </summary>
    public class StudentCollectionService : IStudentCollectionService
    {
        private readonly IMongoCollection<Student> _students;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentCollectionService"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings containing the connection string, database name, and collection name.</param>
        public StudentCollectionService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _students = database.GetCollection<Student>(settings.StudentsCollectionName);
        }

        /// <summary>
        /// Adds a grade to a student's record.
        /// </summary>
        /// <param name="userId">The ID of the student.</param>
        /// <param name="grade">The grade to be added.</param>
        /// <returns>True if the grade was successfully added, otherwise false.</returns>
        public async Task<bool> AddGrade(string userId, Grade grade)
        {
            if (userId == null || grade == null)
                return false;

            var user = await GetById(userId);
            if (user == null)
                return false;

            user.Grades.Add(grade);
            await Update(userId, user);
            return true;
        }

        /// <summary>
        /// Creates a new student record in the database.
        /// </summary>
        /// <param name="entity">The student entity to be created.</param>
        /// <returns>True if the operation was successful, otherwise false.</returns>
        public async Task<bool> Create(Student entity)
        {
            if (entity == null)
                return false;

            var existingStudent = await _students.Find(s => s.Username == entity.Username || s.Email == entity.Email).FirstOrDefaultAsync();

            if (existingStudent != null)
                return false;

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();

            await _students.InsertOneAsync(entity);
            return true;
        }

        /// <summary>
        /// Deletes a student record from the database by their ID.
        /// </summary>
        /// <param name="id">The ID of the student to delete.</param>
        /// <returns>True if the student was deleted successfully, otherwise false.</returns>
        public async Task<bool> Delete(string id)
        {
            var results = await _students.DeleteOneAsync(student => student.Id.ToString() == id);
            return results.IsAcknowledged && results.DeletedCount > 0;
        }

        /// <summary>
        /// Retrieves all student records from the database.
        /// </summary>
        /// <returns>A list of all student records.</returns>
        public async Task<List<Student>> GetAll()
        {
            var result = await _students.FindAsync(student => true);
            return result.ToList();
        }

        /// <summary>
        /// Retrieves a student record by its ID.
        /// </summary>
        /// <param name="id">The ID of the student to retrieve.</param>
        /// <returns>The student record if found, otherwise null.</returns>
        public async Task<Student> GetById(string id)
        {
            return (await _students.FindAsync(student => student.Id.ToString() == id)).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a list of grades for a specific student.
        /// </summary>
        /// <param name="id">The ID of the student whose grades are to be retrieved.</param>
        /// <returns>A list of grades for the student, or null if the student does not exist.</returns>
        public async Task<List<Grade>> GetGrades(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var student = (await _students.FindAsync(student => student.Id == id)).FirstOrDefault();
            return student?.Grades;
        }

        /// <summary>
        /// Updates an existing student record by its ID.
        /// </summary>
        /// <param name="id">The ID of the student to update.</param>
        /// <param name="entity">The student entity containing updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
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

        /// <summary>
        /// Verifies a student's account using their username and password.
        /// </summary>
        /// <param name="username">The username of the student.</param>
        /// <param name="password">The password of the student.</param>
        /// <returns>The student record if the account is verified, otherwise null.</returns>
        public async Task<Student> VerifyAccount(string username, string password)
        {
            var student = (await _students.FindAsync(student => student.Username == username && student.Password == password)).FirstOrDefault();
            return student;
        }

        /// <summary>
        /// Retrieves a summarized dashboard for a specific student, including their grades per course and overall GPA.
        /// </summary>
        /// <param name="studentId">The ID of the student whose dashboard is to be retrieved.</param>
        /// <returns>
        /// A <see cref="DashboardSummaryDto"/> object containing:
        /// - A list of courses with their associated grades and average score,
        /// - The student's overall GPA (average of course averages).
        /// Returns an empty list and GPA 0 if no grades are found.
        /// </returns>
        public async Task<DashboardSummaryDto> GetStudentDashboard(string studentId)
        {
            var grades = await GetGrades(studentId);

            if (grades == null || !grades.Any())
                return new DashboardSummaryDto
                {
                    CourseGrades = new List<CourseGradesDto>(),
                    OverallGPA = 0
                };

            var courseGroups = grades.GroupBy(g => g.CourseId);

            var courseGrades = courseGroups.Select(group =>
            {
                var courseGradesList = group.ToList();
                decimal totalScore = courseGradesList.Sum(g => g.Score);
                decimal totalMaxScore = courseGradesList.Sum(g => g.MaxGrade);
                decimal courseAverage = totalMaxScore == 0 ? 0 : (totalScore / totalMaxScore) * 100;

                return new CourseGradesDto
                {
                    CourseName = group.Key,
                    Grades = courseGradesList,
                    CourseAverage = courseAverage
                };
            }).ToList();

            decimal overallGPA = courseGrades.Average(cg => cg.CourseAverage);

            return new DashboardSummaryDto
            {
                CourseGrades = courseGrades,
                OverallGPA = overallGPA
            };
        }

    }
}
