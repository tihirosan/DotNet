using FluentValidation;
using Library.API.Resources;

namespace Library.API.Validators
{
    public class SaveBookResourceValidator : AbstractValidator<SaveBookResource>
    {
        public SaveBookResourceValidator()
        {
            const int maxLength = 50;
            const string errorMsg = "'Author Id' must be greater than 0.";
            
            RuleFor(m => m.Name).NotEmpty().MaximumLength(maxLength);
            
            RuleFor(m => m.AuthorId).NotEmpty().WithMessage(errorMsg);
        }
    }
}