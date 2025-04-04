using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IEmailService
    {
        Task SendEmail(EmailModel email);
    }
}
