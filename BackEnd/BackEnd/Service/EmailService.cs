using BackEnd.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace BackEnd.Service
{
    // <summary>
    // Service for sending emails using SMTP protocol.
    // Implements the IEmailService interface.
    // </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration object to access email settings.</param>
        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        /// <summary>
        /// Sends an email using the specified email model.
        /// </summary>
        /// <param name="emailModel">The model containing the email details such as recipient, subject, and content.</param>
        public async Task SendEmail(EmailModel emailModel)
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("Smart Grades", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }

}
