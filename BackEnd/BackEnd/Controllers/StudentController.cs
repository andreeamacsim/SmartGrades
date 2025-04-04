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
    // Controller for managing student-related operations, including password reset functionality.
    // </summary>
    [Route("student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentCollectionService _studentService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentController"/> class.
        /// </summary>
        /// <param name="studentService">Service for managing students.</param>
        /// <param name="configuration">Configuration settings.</param>
        /// <param name="emailService">Service for sending emails.</param>
        public StudentController(IStudentCollectionService studentService, IConfiguration configuration, IEmailService emailService)
        {
            _studentService = studentService;
            _config = configuration;
            _emailService = emailService;
        }

        /// <summary>
        /// Sends a password reset email to the student.
        /// </summary>
        /// <param name="email">The email address of the student requesting the reset.</param>
        /// <returns>Returns an HTTP response indicating success or failure of the email sending.</returns>
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
            var emailToken = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');

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

        /// <summary>
        /// Resets the student's password using the reset token.
        /// </summary>
        /// <param name="resetPasswordDto">DTO containing the reset token, new password, and student's email.</param>
        /// <returns>Returns an HTTP response indicating success or failure of the password reset process.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var receivedToken = resetPasswordDto.EmailToken
                .Replace("-", "+")
                .Replace("_", "/");

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
