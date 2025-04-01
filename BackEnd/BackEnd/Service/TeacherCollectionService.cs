﻿using BackEnd.Models;
using BackEnd.Settings;
using MongoDB.Driver;

namespace BackEnd.Service
{
    /// <summary>
    /// Service for managing basic operations on the teacher collection in MongoDB.
    /// Allows creating, updating, deleting, retrieving teachers, adding grades, and verifying teacher accounts.
    /// </summary>
    public class TeacherCollectionService : ITeacherCollectionService
    {
        private readonly IMongoCollection<Teacher> _teachers;
        private readonly IStudentCollectionService _students;
        private readonly ICourseCollectionService _courses;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherCollectionService"/> class.
        /// </summary>
        /// <param name="settings">The MongoDB settings containing the connection string, database name, and collection name.</param>
        /// <param name="studentCollectionService">Service for managing student-related operations.</param>
        /// <param name="courseCollectionService">Service for managing course-related operations.</param>
        public TeacherCollectionService(IMongoDbSettings settings, IStudentCollectionService studentCollectionService, ICourseCollectionService courseCollectionService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _teachers = database.GetCollection<Teacher>(settings.TeachersCollectionName);
            _students = studentCollectionService;
            _courses = courseCollectionService;
        }

        /// <summary>
        /// Adds a grade to a student for a specific course, ensuring that both teacher and student are valid for the course.
        /// </summary>
        /// <param name="grade">The grade to be added.</param>
        /// <returns>True if the grade was added successfully, otherwise false.</returns>
        public async Task<bool> AddGrade(Grade grade)
        {
            if (!await _courses.HasTeacher(grade.TeacherId, grade.CourseId) && !await _courses.HasStudent(grade.StudentId, grade.CourseId))
            {
                return false;
            }

            if (string.IsNullOrEmpty(grade.Id))
                grade.Id = Guid.NewGuid().ToString();

            var result = await _students.AddGrade(grade.StudentId, grade);
            return result;
        }

        /// <summary>
        /// Creates a new teacher record in the database.
        /// </summary>
        /// <param name="entity">The teacher entity to be created.</param>
        /// <returns>True if the operation was successful, otherwise false.</returns>
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

        /// <summary>
        /// Deletes a teacher record from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the teacher to delete.</param>
        /// <returns>True if the teacher was deleted successfully, otherwise false.</returns>
        public async Task<bool> Delete(string id)
        {
            var result = await _teachers.DeleteOneAsync(teacher => teacher.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        /// <summary>
        /// Retrieves all teacher records from the database.
        /// </summary>
        /// <returns>A list of all teacher records.</returns>
        public async Task<List<Teacher>> GetAll()
        {
            var result = await _teachers.FindAsync(_ => true);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Retrieves a teacher record by its ID.
        /// </summary>
        /// <param name="id">The ID of the teacher to retrieve.</param>
        /// <returns>The teacher record if found, otherwise null.</returns>
        public async Task<Teacher?> GetById(string id)
        {
            var result = await _teachers.FindAsync(teacher => teacher.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates an existing teacher record by its ID.
        /// </summary>
        /// <param name="id">The ID of the teacher to update.</param>
        /// <param name="entity">The teacher entity containing updated values.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> Update(string id, Teacher entity)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            entity.Id = id;
            var result = await _teachers.ReplaceOneAsync(teacher => teacher.Id == id, entity);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        /// <summary>
        /// Verifies a teacher's account using their username and password.
        /// </summary>
        /// <param name="username">The username of the teacher.</param>
        /// <param name="password">The password of the teacher.</param>
        /// <returns>The teacher record if the account is verified, otherwise null.</returns>
        public async Task<Teacher> VerifyAccount(string username, string password)
        {
            var teacher = (await _teachers.FindAsync(teacher => teacher.Username == username && teacher.Password == password)).FirstOrDefault();
            return teacher;
        }
    }
}

