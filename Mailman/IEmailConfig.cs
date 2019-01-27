using System;
using FluentValidation.Results;

namespace Mailman
{
    public interface IEmailConfig
    {
        string FromName { get; set; }
        string FromAddress { get; set; }
        string LocalDomain { get; set; }
        string MailServerAddress { get; set; }
        string MailServerPort { get; set; }
        string UserId { get; set; }
        string UserPassword { get; set; }
        ValidationResult Validation { get; }
    }
}