using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackEnd.Controllers;
using BackEnd.Models;
using BackEnd.Service;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace BackEnd.Tests
{
    public class StudentControllerTests
    {
        [Fact]
        public async Task SendEmail_ReturnsOk_WhenEmailExists()
        {
            // Arrange
            var mockStudentService = new Mock<IStudentCollectionService>();
            var mockEmailService = new Mock<IEmailService>();
            var mockLogger = new Mock<ILogger<StudentController>>(); // Mock ILogger
            var mockConfiguration = new Mock<IConfiguration>(); // Mock IConfiguration

            var student = new Student
            {
                Id = "student-01",
                Email = "student1@example.com"
            };

            // Simulate getting the student
            mockStudentService.Setup(s => s.GetAll()).ReturnsAsync(new List<Student> { student });

            // Mock the email sending functionality to prevent actual emails from being sent
            mockEmailService.Setup(es => es.SendEmail(It.IsAny<EmailModel>())).Verifiable();

            // Pass the mocked ILogger and IConfiguration to the controller
            var controller = new StudentController(mockStudentService.Object, mockConfiguration.Object, mockEmailService.Object);

            // Act
            var result = await controller.SendEmail("student1@example.com");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Verify that SendEmail was called
            mockEmailService.Verify(es => es.SendEmail(It.IsAny<EmailModel>()), Times.Once);
        }

        [Fact]
        public async Task SendEmail_ReturnsNotFound_WhenEmailDoesNotExist()
        {
            // Arrange
            var mockStudentService = new Mock<IStudentCollectionService>();
            var mockEmailService = new Mock<IEmailService>();
            var mockLogger = new Mock<ILogger<StudentController>>(); // Mock ILogger
            var mockConfiguration = new Mock<IConfiguration>(); // Mock IConfiguration

            // Simulate no students in the system
            mockStudentService.Setup(s => s.GetAll()).ReturnsAsync(new List<Student>());

            // Pass the mocked ILogger and IConfiguration to the controller
            var controller = new StudentController(mockStudentService.Object, mockConfiguration.Object, mockEmailService.Object);

            // Act
            var result = await controller.SendEmail("nonexistent@example.com");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
