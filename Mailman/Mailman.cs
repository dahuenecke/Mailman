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
using Mailman.Validation;
using FluentValidation.Results;

// Source: https://www.codeproject.com/Articles/1166364/%2FArticles%2F1166364%2FSend-email-with-Net-Core-using-Dependency-Injectio

namespace Mailman
{
    public class Mailman : IMailman
    {
        private readonly IEmailConfig _config;
        private readonly ConfigValidator _configValidator = new ConfigValidator();

        public Mailman(IEmailConfig config)
        {
            _config = config;
            IsValidConfig();
        }

        public async Task SendEmailAsync(string recipient, string subject, string message, bool isHtml = false)
        {
            await SendEmailsAsync(new List<string> { recipient }, subject, message);
        }
        
        public async Task SendEmailsAsync(IEnumerable<string> recipients, string subject, string message, bool isHtml = false)
        {
            IsValidEmail(recipients, subject, message);

            try
            {
                IEnumerable<MimeMessage> emails = recipients.Select(r => CreateMessage(r, subject, message, isHtml));
                await SendAsync(emails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private MimeMessage CreateMessage(string recipient, string subject, string message, bool isHtml)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config.FromName, _config.FromAddress));
            email.Subject = subject;
            email.To.Add(new MailboxAddress("", recipient));
            
            var bodyBuilder = new BodyBuilder();
            if (isHtml)
                bodyBuilder.HtmlBody =  message;
            else
                bodyBuilder.TextBody = message;
            
            email.Body = bodyBuilder.ToMessageBody();

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

        private void IsValidConfig()
        {
            ValidationResult validation = _configValidator.Validate(_config);
            if(!validation.IsValid) throw new ArgumentException(validation.ToString("~"));
        }

        private void IsValidEmail(IEnumerable<string> recipients, string subject, string message)
        {
            IEmail email = new Email(recipients, subject, message);
            if(!email.Validation.IsValid) throw new ArgumentException(email.Validation.ToString("~"));
        }
    }
}