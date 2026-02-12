using AbsoluteCinema.Application.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace AbsoluteCinema.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = CreateEmailMessage(to, subject);

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            await SendAsync(email);
        }

        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentData, string fileName)
        {
            var email = CreateEmailMessage(to, subject);

            var builder = new BodyBuilder();
            builder.HtmlBody = body;

            if (attachmentData != null && attachmentData.Length > 0)
            {
                builder.Attachments.Add(fileName, attachmentData, ContentType.Parse("application/pdf"));
            }

            email.Body = builder.ToMessageBody();

            await SendAsync(email);
        }

        private MimeMessage CreateEmailMessage(string to, string subject)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Absolute Cinema", _config["EmailSettings:SenderEmail"]));

            email.To.Add(new MailboxAddress("", to));

            email.Subject = subject;

            return email;
        }

        private async Task SendAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync(
                    _config["EmailSettings:Server"],
                    int.Parse(_config["EmailSettings:Port"]),
                    SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _config["EmailSettings:SenderEmail"],
                    _config["EmailSettings:AppPassword"]
                );

                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }
    }
}