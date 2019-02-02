using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mailman.Net.Smtp
{
    public interface IMailman
    {
        Task SendEmailAsync(string recipient, string subject, string message, bool isHtml);
        Task SendEmailsAsync(IEnumerable<string> recipients, string subject, string message, bool isHtml);
    }
}
