using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("grade")]
    public class GradeController : ControllerBase
    {
        private readonly IStudentCollectionService _studentCollectionService;
        private ITeacherCollectionService _teacherCollectionService;

        public GradeController(IStudentCollectionService studentCollectionService, ITeacherCollectionService teacherCollectionService)
        {
            _studentCollectionService = studentCollectionService;
            _teacherCollectionService = teacherCollectionService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetStudentGrades(string id)
        {
            var results= await _studentCollectionService.GetGrades(id);
            if (results == null)
                return NotFound();
            return Ok(results);
        }
        [HttpPost("add-grade")]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            var results= await _teacherCollectionService.AddGrade(grade);
            if(results)
                return Ok(results);
            return BadRequest();
        }

    }
}
