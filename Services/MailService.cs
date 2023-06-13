﻿using HelloChat.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace HelloChat.Services
{
    public class MailService: IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public async Task SendMailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
