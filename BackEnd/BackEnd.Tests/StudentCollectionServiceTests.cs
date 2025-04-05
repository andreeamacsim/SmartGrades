using Xunit;
using Moq;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackEnd.Service;
using BackEnd.Models;
using BackEnd.Settings;

namespace BackEnd.Tests
{
    public class StudentCollectionServiceTests
    {
        private readonly Mock<IMongoCollection<Student>> _mockCollection;
        private readonly StudentCollectionService _service;

        public StudentCollectionServiceTests()
        {
            _mockCollection = new Mock<IMongoCollection<Student>>();
            var mockSettings = new Mock<IMongoDbSettings>();
            mockSettings.Setup(s => s.ConnectionString).Returns("mongodb://localhost:27017");
            mockSettings.Setup(s => s.DatabaseName).Returns("test-db");
            mockSettings.Setup(s => s.StudentsCollectionName).Returns("students");

            // Use a real client but never hit a real database, this is just to pass through constructor.
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test-db");
            var service = new StudentCollectionService(mockSettings.Object);

            // Replace the internal collection with our mock using reflection (just for this simple case)
            typeof(StudentCollectionService)
                .GetField("_students", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(service, _mockCollection.Object);

            _service = service;
        }

        [Fact]
        public async Task AddGrade_ReturnsFalse_WhenStudentNotFound()
        {
            // Arrange
            var userId = "123";
            var grade = new Grade { Score = 90, MaxGrade = 100 };

            var mockCursor = new Mock<IAsyncCursor<Student>>();
            mockCursor.Setup(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(false);
            mockCursor.Setup(c => c.Current).Returns(new List<Student>());

            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Student>>(),
                    It.IsAny<FindOptions<Student, Student>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _service.AddGrade(userId, grade);

            // Assert
            Assert.False(result);
        }
    }
}
