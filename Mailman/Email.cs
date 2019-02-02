using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Mailman.Validation;

namespace Mailman
{
    public class Email : IEmail
    {
        private EmailValidator _validator = new EmailValidator();
        public Email(IEnumerable<string> recipients, string subject, string message, bool isHtml = false)
        {
            Recipients = recipients;
            Subject = subject;
            Message = message;
            IsHtml = isHtml;
            Validate();   
        }
        public IEnumerable<string> Recipients { get; set; } 
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsHtml { get; set; }
        public ValidationResult Validation { get; private set; }

        private void Validate()
        {
            Validation = _validator.Validate(this);
        }
    }
}