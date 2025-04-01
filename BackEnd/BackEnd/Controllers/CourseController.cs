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
        [HttpGet("id")]
        public async Task<IActionResult> GetCoursesForTeacher(string id)
        {
            var results =await courseCollectionService.GetTeacherCoursesList(id);
            if(results==null)
                return BadRequest();
            return Ok(results);
        }
        [HttpGet("students-from-course")]
        public async Task<IActionResult> GetStudentsFromCourse(string courseId)
        {
            var results = await courseCollectionService.GetStudentsFromCourse(courseId);
            if(results==null)
                return BadRequest();
            return Ok(results);
        }
    }
}
