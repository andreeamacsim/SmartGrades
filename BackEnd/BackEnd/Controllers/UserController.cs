﻿using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IStudentCollectionService studentCollectionService;
        private ITeacherCollectionService teacherCollectionService;
        public UserController(IStudentCollectionService studentCollectionService, ITeacherCollectionService teacherCollectionService)
        {
            this.studentCollectionService = studentCollectionService;
            this.teacherCollectionService = teacherCollectionService;
        }
        [HttpPost("/student")]
        public async Task<IActionResult> CreateUser([FromBody] Student student)
        {
            var results = await studentCollectionService.Create(student);
            if (!results)
                return BadRequest();
            return Ok(results);
        }
        [HttpPost("/teacher")]
        public async Task<IActionResult> CreateTeacher([FromBody] Teacher teacher)
        {
            var results = await teacherCollectionService.Create(teacher);
            if (!results)
                return BadRequest();
            return Ok(results);
        }

        [HttpPost("/student/authenticate")]
        public async Task<IActionResult> AuthenitcateStudent([FromBody] VerifyUser student)
        {
            if (student == null)
                return BadRequest();
            var user = await studentCollectionService.VerifyAccount(student.Username, student.Password);
            if (user != null)
            {
                return Ok(new
                {
                    Message = "Login succesed!"
                });
            }
            return NotFound(new { Message = "Username or password is incorect" });
        }
        [HttpPost("/teacher/authenticate")]
        public async Task<IActionResult> AuthenitcateTeacher([FromBody] VerifyUser teacher)
        {
            if (teacher == null)
                return BadRequest();
            var user = await studentCollectionService.VerifyAccount(teacher.Username, teacher.Password);
            if (user != null)
            {
                return Ok(new
                {
                    Message = "Login succesed!"
                });
            }
            return NotFound(new { Message = "Username or password is incorect" });
        }
    }
}
