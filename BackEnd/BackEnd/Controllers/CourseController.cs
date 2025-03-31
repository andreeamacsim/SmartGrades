using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController :ControllerBase
    {
        private readonly ICourseCollectionService courseCollectionService;
        public CourseController(ICourseCollectionService courseCollectionService)
        {
            this.courseCollectionService = courseCollectionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            var results= await courseCollectionService.Create(course);
            if(results)
                return Ok(results);
            return BadRequest();

        }
    }
}
