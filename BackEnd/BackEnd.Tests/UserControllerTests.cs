using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Service;
using System.Linq;
using BackEnd.Settings;
using BackEnd.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BackEnd.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task CreateUser_ReturnsOk_WhenStudentIsValid()
        {
            // Arrange
            var mockStudentService = new Mock<IStudentCollectionService>();
            var mockTeacherService = new Mock<ITeacherCollectionService>();
            var student = new Student
            {
                Id = "student-01",
                Username = "student1",
                Email = "student1@example.com",
                Password = "password123"
            };

            // Simulăm crearea studentului cu succes
            mockStudentService.Setup(s => s.Create(It.IsAny<Student>())).ReturnsAsync(true);

            var controller = new UserController(mockStudentService.Object, mockTeacherService.Object);

            // Act
            var result = await controller.CreateUser(student);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task AuthenticateStudent_ReturnsNotFound_WhenCredentialsAreInvalid()
        {
            // Arrange
            var mockStudentService = new Mock<IStudentCollectionService>();
            var mockTeacherService = new Mock<ITeacherCollectionService>();

            var studentCredentials = new VerifyUser
            {
                Username = "student1",
                Password = "wrongpassword"
            };

            // Simulăm autentificarea care nu găsește studentul (returnează null)
            mockStudentService.Setup(s => s.VerifyAccount(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync((Student)null);

            var controller = new UserController(mockStudentService.Object, mockTeacherService.Object);
            // Act
            var result = await controller.AuthenticateStudent(studentCredentials);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var jsonResult = JsonConvert.SerializeObject(notFoundResult.Value);
            // JSON așteptat
            var expectedJson = JsonConvert.SerializeObject(new
            {
                Message = "Username or password is incorect"
            });

            // Comparăm JSON-urile
            Assert.Equal(expectedJson, jsonResult);
        }
    }
}
