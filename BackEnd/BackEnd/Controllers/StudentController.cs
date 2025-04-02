using Microsoft.AspNetCore.Mvc;
using BackEnd.Service;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BackEnd.Models;
using BackEnd.Helpers;
using BackEnd.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace BackEnd.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentCollectionService _studentService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public StudentController(IStudentCollectionService studentService, IConfiguration configuration, IEmailService emailService)
        {
            _studentService = studentService;
            _config = configuration;
            _emailService = emailService;
        }

        [HttpPost("/student")]
        public async Task<IActionResult> CreateUser([FromBody] Student student)
        {
            var results = await _studentService.Create(student);
            if (!results)
                return BadRequest();
            return Ok(results);
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var studentList = await _studentService.GetAll();
            var student = studentList.FirstOrDefault(s => s.Email == email);
            if (student == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            student.ResetPasswordToken = emailToken;
            student.ResetPasswordExpiry = DateTime.UtcNow.AddMinutes(15);

            await _studentService.Update(student.Id, student);

            string from = _config["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Email sent successfully"
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var studentList = await _studentService.GetAll();
            var student = studentList.FirstOrDefault(a => a.Email == resetPasswordDto.Email);
            if (student == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist"
                });
            }

            var tokenCode = student.ResetPasswordToken;
            DateTime emailTokenExpiry = student.ResetPasswordExpiry;

            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.UtcNow)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid reset link"
                });
            }

            var passwordHasher = new PasswordHasher<Student>();
            student.Password = resetPasswordDto.NewPassword;

            await _studentService.Update(student.Id, student);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Password reset successfully"
            });
        }

    }
}
