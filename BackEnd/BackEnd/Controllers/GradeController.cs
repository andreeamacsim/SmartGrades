using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    /// <summary>
    /// Controller for managing grades.
    /// </summary>
    //[Authorize]
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


        // <summary>
        /// Retrieves all grades assigned by a specific teacher.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher whose grades should be retrieved.</param>
        /// <returns>Returns the teacher's assigned grades if found, otherwise returns NotFound.</returns>
        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetTeacherGrades(string teacherId)
        {
            var results = await _teacherCollectionService.GetTeacherGrades(teacherId);
            return Ok(results);
        }

        /// <summary>
        /// Updates an existing grade.
        /// </summary>
        /// <param name="grade">The grade with updated information.</param>
        /// <returns>Returns Ok if the grade is successfully updated, otherwise returns BadRequest.</returns>
        [HttpPut("update-grade")]
        public async Task<IActionResult> UpdateGrade([FromBody] Grade grade)
        {
            var result = await _teacherCollectionService.UpdateGrade(grade);
            if (result)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Deletes a grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the grade to delete.</param>
        /// <returns>Returns Ok if the grade is successfully deleted, otherwise returns BadRequest.</returns>
        [HttpDelete("delete-grade/{gradeId}")]
        public async Task<IActionResult> DeleteGrade(string gradeId)
        {
            var result = await _teacherCollectionService.DeleteGrade(gradeId);
            if (result)
                return Ok(result);
            return BadRequest();
        }

        /// <summary>
        /// Retrieves the dashboard data for a student, including relevant academic information.
        /// </summary>
        /// <param name="studentId">The student's ID.</param>
        /// <returns>Returns the student's dashboard data.</returns>
        [HttpGet("dashboard/{studentId}")]
        public async Task<IActionResult> GetStudentDashboard(string studentId)
        {
            var result = await _studentCollectionService.GetStudentDashboard(studentId);
            return Ok(result);
        }
    }
}
