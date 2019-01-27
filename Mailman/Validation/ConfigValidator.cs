using FluentValidation;

namespace Mailman.Validation
{
    public class ConfigValidator : AbstractValidator<IEmailConfig>
    {
        public ConfigValidator()
        {
            RuleFor(x => x.FromAddress).EmailAddress();
            RuleFor(x => x.FromName).NotEmpty();
            RuleFor(x => x.LocalDomain).NotEmpty();
            RuleFor(x => x.MailServerAddress).NotEmpty();
            RuleFor(x => x.MailServerPort).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserPassword).NotEmpty();
        }
    }
}