using HelloChat.Models;

namespace HelloChat.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest mailRequest);
    }
}
