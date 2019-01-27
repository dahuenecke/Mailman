using FluentValidation;

namespace Mailman.Validation
{
   public class EmailValidator : AbstractValidator<IEmail>
   {
       public EmailValidator()
       {
           RuleSet("Recipients", () =>  
           {
            RuleForEach(x => x.Recipients).NotEmpty();
            RuleForEach(x => x.Recipients).EmailAddress();               
           });
           RuleFor(x => x.Subject).NotEmpty();
           RuleFor(x => x.Message).NotEmpty();
       }
   } 
}