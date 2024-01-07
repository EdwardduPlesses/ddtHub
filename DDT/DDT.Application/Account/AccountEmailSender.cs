using System;
using System.IO;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DDT.Domain.Common.Constants;
using Intent.RoslynWeaver.Attributes;
using Microsoft.Extensions.Configuration;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.AspNetCore.Identity.AccountController.AccountEmailSender", Version = "1.0")]

namespace DDT.Application.Account;

public class AccountEmailSender : IAccountEmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly IConfiguration _configuration;

    [IntentManaged(Mode.Merge)]
    public AccountEmailSender(SmtpClient smtpClient,
                              IConfiguration configuration)
    {
        _smtpClient = smtpClient;
        _configuration = configuration;
    }

    [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
    public async Task SendEmailConfirmationRequest(string email, string userId, string code)
    {

        var env = _configuration.GetValue<string>("EmailSettings:Environment") ?? string.Empty;
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Combine(basePath, "EmailTemplate", "email-template.html");
        var htmlContent = File.ReadAllText(filePath);
        htmlContent = htmlContent.Replace("{env}", HtmlEncoder.Default.Encode(env));
        htmlContent = htmlContent.Replace("{userId}", HtmlEncoder.Default.Encode(userId));
        htmlContent = htmlContent.Replace("{code}", HtmlEncoder.Default.Encode(code));
        var mailMessage = new MailMessage
        {
            From = new MailAddress(EmailConstants.EmailSender),
            Subject = EmailConstants.EmailSubject,
            Body = htmlContent,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);

        await _smtpClient.SendMailAsync(mailMessage);
    }
}