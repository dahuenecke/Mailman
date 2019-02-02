using System;
using FluentValidation.Results;
using Mailman.Net.Smtp.Validation;

namespace Mailman.Net.Smtp
{
    public class EmailConfig : IEmailConfig
    {
        private ConfigValidator _validator = new ConfigValidator();
        
        public EmailConfig()
        {
            Validate();
        }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string LocalDomain { get; set; }
        public string MailServerAddress { get; set; }
        public string MailServerPort { get; set; }
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public ValidationResult Validation { get; private set; }

        private void Validate()
        {
          Validation = _validator.Validate(this);   
        }
    }
}
