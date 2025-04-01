using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Service;
using MongoDB.Driver;
using System.Linq;
using BackEnd.Settings;

namespace BackEnd.Tests
{
    public class TeacherCollectionServiceTests
    {
        private readonly Mock<IMongoDbSettings> _mockSettings;
        private readonly Mock<IStudentCollectionService> _mockStudentService;
        private readonly Mock<ICourseCollectionService> _mockCourseService;
        private readonly Mock<IMongoCollection<Teacher>> _mockTeacherCollection;
        private readonly TeacherCollectionService _teacherService;

        public TeacherCollectionServiceTests()
        {
            _mockSettings = new Mock<IMongoDbSettings>();
            _mockStudentService = new Mock<IStudentCollectionService>();
            _mockCourseService = new Mock<ICourseCollectionService>();

            // Mock settings for MongoDB
            _mockSettings.Setup(s => s.ConnectionString).Returns("mongodb://localhost:27017");
            _mockSettings.Setup(s => s.DatabaseName).Returns("CatalogTests");
            _mockSettings.Setup(s => s.TeachersCollectionName).Returns("Teachers");

            // Mock the MongoDB collection
            _mockTeacherCollection = new Mock<IMongoCollection<Teacher>>();

            // Mock the MongoDB database and client
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Teacher>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(_mockTeacherCollection.Object);
            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(mockDatabase.Object);

            // Now initialize the service using the mocked database and collection
            _teacherService = new TeacherCollectionService(_mockSettings.Object, _mockStudentService.Object, _mockCourseService.Object);
        }

        [Fact]
        public async Task CreateTeacher_ShouldReturnTrue_WhenTeacherIsValid()
        {
            // Arrange
            var teacher = new Teacher
            {
                Id="teacher-01",
                Username = "teacher1",
                Email = "teacher1@example.com",
                Password = "password123"
            };

            // Mock InsertOneAsync behavior to simulate inserting a teacher without actually interacting with a real database
            _mockTeacherCollection.Setup(c => c.InsertOneAsync(It.IsAny<Teacher>(), null, default)).Returns(Task.CompletedTask);

            // Act
            var result = await _teacherService.Create(teacher);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task VerifyTeacherAccount_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange: Invalid credentials
            var teacher = new Teacher
            {
                Username = "teacher1",
                Password = "password123",
                Id = "teacher-01",
                Email = "teacher1@example.com"
            };

            var teacherList = new List<Teacher> { teacher };

            // Mock FindAsync behavior to return no teacher for invalid credentials
            var mockCursor = new Mock<IAsyncCursor<Teacher>>();
            mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<System.Threading.CancellationToken>())).Returns(false);
            mockCursor.Setup(x => x.Current).Returns(new List<Teacher>());
            _mockTeacherCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Teacher>>(), It.IsAny<FindOptions<Teacher>>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(mockCursor.Object);

            // Act: Test invalid credentials
            var result = await _teacherService.VerifyAccount("teacher2", "wrongpassword");

            // Assert: Verify that result is null when credentials are invalid
            Assert.Null(result);
        }
    }
}
