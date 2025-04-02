using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IEmailService
    {
        void SendEmail(EmailModel email);
    }
}
