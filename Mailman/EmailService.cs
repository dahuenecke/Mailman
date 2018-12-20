using System;
using System.Net;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using Microsoft.Extensions.Options;

// Source: https://www.codeproject.com/Articles/1166364/%2FArticles%2F1166364%2FSend-email-with-Net-Core-using-Dependency-Injectio

namespace Mailman
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _config;

        public EmailService(IOptions<EmailConfig> config)
        {
            _config = config.Value;
        }

        public async Task SendEmailAsync(string recipient, string subject, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_config.FromName, _config.FromAddress));
                emailMessage.To.Add(new MailboxAddress("", recipient));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                using(var client = new SmtpClient())
                {
                    client.LocalDomain = _config.LocalDomain;

                    await client.ConnectAsync(_config.MailServerAddress, Convert.ToInt32(_config.MailServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(_config.UserId, _config.UserPassword));
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
