using Application.DataTransferObjects;
using FluentValidation;


namespace Application.Validations
{
    public class StateCreateValidator : AbstractValidator<StateCreateDto>
    {
        public StateCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Name is required.");
        }
    }
}