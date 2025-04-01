using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BackEnd.Controllers;
using BackEnd.Service;
using BackEnd.Models;
using System.Collections.Generic;

namespace BackEnd.Tests
{
    public class GradeControllerTests
    {
        [Fact]
        public async Task GetStudentGrades_ReturnsOk_WhenGradesExist()
        {
            // Arrange
            var mockStudentService = new Mock<IStudentCollectionService>();
            var mockTeacherService = new Mock<ITeacherCollectionService>();
            var studentId = "123";
            var grades = new List<Grade> { new Grade { Id = "1", StudentId = studentId, Score = 95, MaxGrade = 100 } };

            mockStudentService.Setup(s => s.GetGrades(studentId)).ReturnsAsync(grades);

            var controller = new GradeController(mockStudentService.Object, mockTeacherService.Object);

            // Act
            var result = await controller.GetStudentGrades(studentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(grades, okResult.Value);
        }

        [Fact]
        public async Task AddGrade_ReturnsOk_WhenGradeIsAdded()
        {
            // Arrange
            var mockStudentService = new Mock<IStudentCollectionService>();
            var mockTeacherService = new Mock<ITeacherCollectionService>();
            var grade = new Grade { Id = "1", StudentId = "123", Score = 90, MaxGrade = 100 };

            mockTeacherService.Setup(s => s.AddGrade(grade)).ReturnsAsync(true);

            var controller = new GradeController(mockStudentService.Object, mockTeacherService.Object);

            // Act
            var result = await controller.AddGrade(grade);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
