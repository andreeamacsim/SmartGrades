using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    /// <summary>
    /// Controller for managing grades.
    /// </summary>
    [ApiController]
    [Route("grade")]
    public class GradeController : ControllerBase
    {
        private readonly IStudentCollectionService _studentCollectionService;
        private ITeacherCollectionService _teacherCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeController"/> class.
        /// </summary>
        /// <param name="studentCollectionService">Service for managing student-related operations.</param>
        /// <param name="teacherCollectionService">Service for managing teacher-related operations.</param>
        public GradeController(IStudentCollectionService studentCollectionService, ITeacherCollectionService teacherCollectionService)
        {
            _studentCollectionService = studentCollectionService;
            _teacherCollectionService = teacherCollectionService;
        }

        /// <summary>
        /// Retrieves the grades of a student by their ID.
        /// </summary>
        /// <param name="id">The student's ID.</param>
        /// <returns>Returns the student's grades if found, otherwise returns NotFound.</returns>
        [HttpGet("id")]
        public async Task<IActionResult> GetStudentGrades(string id)
        {
            var results = await _studentCollectionService.GetGrades(id);
            if (results == null)
                return NotFound();
            return Ok(results);
        }

        /// <summary>
        /// Adds a grade for a student.
        /// </summary>
        /// <param name="grade">The grade to be added.</param>
        /// <returns>Returns Ok if the grade is successfully added, otherwise returns BadRequest.</returns>
        [HttpPost("add-grade")]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            var results = await _teacherCollectionService.AddGrade(grade);
            if (results)
                return Ok(results);
            return BadRequest();
        }
    }
}
