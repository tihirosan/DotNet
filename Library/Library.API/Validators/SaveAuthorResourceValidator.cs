using FluentValidation;
using Library.API.Resources;

namespace Library.API.Validators
{
    public class SaveAuthorResourceValidator : AbstractValidator<SaveAuthorResource>
    {
        public SaveAuthorResourceValidator()
        {
            const int maxLength = 50;
            
            RuleFor(a => a.Name).NotEmpty().MaximumLength(maxLength);
        }
    }
}