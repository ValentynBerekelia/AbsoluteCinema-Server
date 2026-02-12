namespace AbsoluteCinema.Application.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentData, string fileName);
}