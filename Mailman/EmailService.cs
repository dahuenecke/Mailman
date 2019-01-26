using System;
using System.Net;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

// Source: https://www.codeproject.com/Articles/1166364/%2FArticles%2F1166364%2FSend-email-with-Net-Core-using-Dependency-Injectio

namespace Mailman
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfig _config;

        public EmailService(IEmailConfig config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string recipient, string subject, string message)
        {
            await SendEmailsAsync(new List<string> { recipient }, subject, message);
        }
        
        public async Task SendEmailsAsync(IEnumerable<string> recipients, string subject, string message)
        {
            Email email = new Email(recipients, subject, message);
            if(!email.Validation.IsValid) throw new ArgumentException(email.Validation.ToString("~"));

            try
            {
                IEnumerable<MimeMessage> emails = recipients.Select(r => CreateMessage(r, subject, message));
                await SendAsync(emails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private MimeMessage CreateMessage(string recipient, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config.FromName, _config.FromAddress));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };
            email.To.Add(new MailboxAddress("", recipient));
            return email;
        }

        private async Task SendAsync(IEnumerable<MimeMessage> emails)
        {
            using (var client = new SmtpClient())
            {
                client.LocalDomain = _config.LocalDomain;

                await client.ConnectAsync(_config.MailServerAddress, Convert.ToInt32(_config.MailServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                await client.AuthenticateAsync(new NetworkCredential(_config.UserId, _config.UserPassword));
                await Task.WhenAll(emails.Select(email => client.SendAsync(email)));
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}

/*
Config.cs

"Email": {
    "FromName": "<fromname>",
    "FromAddress": "<fromaddress>",

    "LocalDomain": "<localdomain>",

    "MailServerAddress": "<mailserveraddress>",
    "MailServerPort": "<mailserverport>",

    "UserId": "<userid>",
    "UserPassword": "<userpasword>"
  }
*/

/*
Startup.cs

public void ConfigureServices(IServiceCollection services)
{
  ...
  // Read email settings
  services.Configure<EmailConfig>(Configuration.GetSection("Email"));
   
  // Register email service 
  services.AddTransient<IEmailService, EmailService>();
  ...
}
*/

/*
Controller DI   

public class HomeController : Controller
{
    private readonly IEmailService _emailService;

    public HomeController(IEmailService emailService)
    {
        _emailService = emailService;
    }
}

await _emailService.SendEmailAsync(recipient, subject, message);

*/
