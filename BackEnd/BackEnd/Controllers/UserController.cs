using BackEnd.Helpers;
using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    // <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IStudentCollectionService studentCollectionService;
        private ITeacherCollectionService teacherCollectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="studentCollectionService">Service for managing students.</param>
        /// <param name="teacherCollectionService">Service for managing teachers.</param>
        public UserController(IStudentCollectionService studentCollectionService, ITeacherCollectionService teacherCollectionService)
        {
            this.studentCollectionService = studentCollectionService;
            this.teacherCollectionService = teacherCollectionService;
        }

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="student">The student to create.</param>
        /// <returns>Returns an HTTP response indicating success or failure.</returns>
        [AllowAnonymous]
        [HttpPost("/student")]
        public async Task<IActionResult> CreateUser([FromBody] Student student)
        {
            var results = await studentCollectionService.Create(student);
            if (!results)
                return BadRequest();
            return Ok(results);
        }

        /// <summary>
        /// Creates a new teacher.
        /// </summary>
        /// <param name="teacher">The teacher to create.</param>
        /// <returns>Returns an HTTP response indicating success or failure.</returns>
        [AllowAnonymous]
        [HttpPost("/teacher")]
        public async Task<IActionResult> CreateTeacher([FromBody] Teacher teacher)
        {
            var results = await teacherCollectionService.Create(teacher);
            if (!results)
                return BadRequest();
            return Ok(results);
        }

        /// <summary>
        /// Authenticates a student.
        /// </summary>
        /// <param name="student">The student's credentials.</param>
        /// <returns>Returns the student data if authentication is successful, otherwise returns NotFound.</returns>
        [AllowAnonymous]
        [HttpPost("/student/authenticate")]
        public async Task<IActionResult> AuthenticateStudent([FromBody] VerifyUser student)
        {
            if (student == null)
                return BadRequest();
            var user = await studentCollectionService.VerifyAccount(student.Username, student.Password);
            if (user != null)
            {
                user.Token = Token.CreateJWTToken(user);
                return Ok(new
                {
                    Token = user.Token,
                    Message = "Login succesed!"
                });
            }
            return NotFound(new { Message = "Username or password is incorect" });
        }

        /// <summary>
        /// Authenticates a teacher.
        /// </summary>
        /// <param name="teacher">The teacher's credentials.</param>
        /// <returns>Returns the teacher data if authentication is successful, otherwise returns NotFound.</returns>
        [AllowAnonymous]
        [HttpPost("/teacher/authenticate")]
        public async Task<IActionResult> AuthenticateTeacher([FromBody] VerifyUser teacher)
        {
            if (teacher == null)
                return BadRequest();
            var user = await teacherCollectionService.VerifyAccount(teacher.Username, teacher.Password);
            if (user != null)
            {
                user.Token = Token.CreateJWTToken(user);
                return Ok(new
                {
                    Token = user.Token,
                    Message = "Login succesed!"
                });
            }
            return NotFound(new { Message = "Username or password is incorect" });
        }

        /// <summary>
        /// Updates an existing teacher.
        /// </summary>
        /// <param name="teacher">The teacher to update.</param>
        /// <returns>Returns an HTTP response indicating success or failure.</returns>
        [HttpPut("/teacher")]
        public async Task<IActionResult> UpdateTeacher([FromBody] Teacher teacher)
        {
            var results = await teacherCollectionService.Update(teacher.Id, teacher);
            if (results)
                return Ok(results);
            return BadRequest();
        }

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="student">The student to update.</param>
        /// <returns>Returns an HTTP response indicating success or failure.</returns>
        [HttpPut("/student")]
        public async Task<IActionResult> UpdateStudent([FromBody] Student student)
        {
            var results = await studentCollectionService.Update(student.Id, student);
            if (results)
                return Ok(results);
            return BadRequest();
        }
    }
}
