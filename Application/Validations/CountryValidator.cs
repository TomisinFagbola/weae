using Application.DataTransferObjects;
using FluentValidation;


namespace Application.Validations
{
    public class CountryCreateValidator : AbstractValidator<CountryCreateDto>
    {
        public CountryCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Name is required.");
            RuleFor(x => x.Continent).NotEmpty().NotNull().WithMessage("Code is required.");

        }
    }

    public class CountryUpdateValidator : AbstractValidator<CountryUpdateDto>
    {
        public CountryUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Name is required.");
            RuleFor(x => x.Continent).NotEmpty().NotNull().WithMessage("Code is required.");

        }
    }
}
