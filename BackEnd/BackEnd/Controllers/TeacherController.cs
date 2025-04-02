using BackEnd.Helpers;
using BackEnd.Models;
using BackEnd.Models.Dto;
using BackEnd.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherCollectionService _teacherService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public TeacherController(ITeacherCollectionService teacherService, IConfiguration configuration, IEmailService emailService)
        {
            _teacherService = teacherService;
            _config = configuration;
            _emailService = emailService;
        }

        [HttpPost("/teacher/send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var teacherList = await _teacherService.GetAll();
            var teacher = teacherList.FirstOrDefault(t => t.Email == email);
            if (teacher == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            teacher.ResetPasswordToken = emailToken;
            teacher.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            await _teacherService.Update(teacher.Id, teacher);

            string from = _config["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            return Ok(new
            {
                StatusCode = 200,
                Message = "Email sent successfully"
            });
        }

        [HttpPost("/teacher/reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var teacherList = await _teacherService.GetAll();
            var teacher = teacherList.FirstOrDefault(t => t.Email == resetPasswordDto.Email);
            if (teacher == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist"
                });
            }

            var tokenCode = teacher.ResetPasswordToken;
            DateTime emailTokenExpiry = teacher.ResetPasswordExpiry;

            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid reset link"
                });
            }

            teacher.Password = resetPasswordDto.NewPassword;

            await _teacherService.Update(teacher.Id, teacher);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Password reset successfully"
            });
        }
    }

}
