using System;
using System.Threading.Tasks;

namespace Mailman
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipient, string subject, string message);
    }
}
