using BackEnd.Helpers;
using BackEnd.Models;
using BackEnd.Models.Dto;
using BackEnd.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace BackEnd.Controllers
{
    // <summary>
    // Controller for managing teacher-related operations, including password reset functionality.
    // </summary>
    [Route("teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherCollectionService _teacherService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherController"/> class.
        /// </summary>
        /// <param name="teacherService">Service for managing teachers.</param>
        /// <param name="configuration">Configuration settings.</param>
        /// <param name="emailService">Service for sending emails.</param>
        public TeacherController(ITeacherCollectionService teacherService, IConfiguration configuration, IEmailService emailService)
        {
            _teacherService = teacherService;
            _config = configuration;
            _emailService = emailService;
        }

        /// <summary>
        /// Sends a password reset email to the teacher.
        /// </summary>
        /// <param name="email">The email address of the teacher requesting the reset.</param>
        /// <returns>Returns an HTTP response indicating success or failure of the email sending.</returns>
        [HttpPost("send-reset-email/{email}")]
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
            var emailToken = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');

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

        /// <summary>
        /// Resets the teacher's password using the reset token.
        /// </summary>
        /// <param name="resetPasswordDto">DTO containing the reset token, new password, and teacher's email.</param>
        /// <returns>Returns an HTTP response indicating success or failure of the password reset process.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var receivedToken = resetPasswordDto.EmailToken
                .Replace("-", "+")
                .Replace("_", "/");

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
