using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("grade")]
    public class GradeController : ControllerBase
    {
        private readonly IStudentCollectionService _studentCollectionService;
        public GradeController(IStudentCollectionService studentCollectionService)
        {
            _studentCollectionService = studentCollectionService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetStudentGrades(string id)
        {
            var results= await _studentCollectionService.GetGrades(id);
            if (results == null)
                return NotFound();
            return Ok(results);
        }

    }
}
