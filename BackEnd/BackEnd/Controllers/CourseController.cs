using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{ /// <summary>
  /// Controller for managing courses.
  /// </summary>
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseCollectionService courseCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseController"/> class.
        /// </summary>
        /// <param name="courseCollectionService">Service for managing courses.</param>
        public CourseController(ICourseCollectionService courseCollectionService)
        {
            this.courseCollectionService = courseCollectionService;
        }

        /// <summary>
        /// Adds a new course.
        /// </summary>
        /// <param name="course">The course to be added.</param>
        /// <returns>Returns Ok if the course is successfully added, otherwise returns BadRequest.</returns>
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            var results = await courseCollectionService.Create(course);
            if (results)
                return Ok(results);
            return BadRequest();
        }

        /// <summary>
        /// Retrieves the list of courses assigned to a teacher.
        /// </summary>
        /// <param name="id">The teacher's ID.</param>
        /// <returns>Returns the list of courses if found, otherwise returns BadRequest.</returns>
        [HttpGet("teacher-id")]
        public async Task<IActionResult> GetCoursesForTeacher(string id)
        {
            var results = await courseCollectionService.GetTeacherCoursesList(id);
            if (results == null)
                return BadRequest();
            return Ok(results);
        }

        /// <summary>
        /// Retrieves a specific course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>Returns the course if found, otherwise returns BadRequest.</returns>

        [HttpGet("id")]
        public async Task<IActionResult> GetCourses(string id)
        {
            var results = await courseCollectionService.GetById(id);
            if (results == null)
                return BadRequest();
            return Ok(results);
        }

        /// <summary>
        /// Retrieves the list of students enrolled in a specific course.
        /// </summary>
        /// <param name="courseId">The course ID.</param>
        /// <returns>Returns the list of students if found, otherwise returns BadRequest.</returns>
        [HttpGet("students-from-course")]
        public async Task<IActionResult> GetStudentsFromCourse(string courseId)
        {
            var results = await courseCollectionService.GetStudentsFromCourse(courseId);
            if (results == null)
                return BadRequest();
            return Ok(results);
        }

        /// <summary>
        /// Retrieves the list of courses that a student is enrolled in.
        /// </summary>
        /// <param name="studentId">The student's ID.</param>
        /// <returns>Returns the list of courses if found, otherwise returns BadRequest.</returns>
        [HttpGet("student-courses")]
        public async Task<IActionResult> GetCoursesForStudent(string studentId)
        {
            var results = await courseCollectionService.GetStudentCoursesList(studentId);
            if (results == null)
                return BadRequest();
            return Ok(results);
        }

    }
}
