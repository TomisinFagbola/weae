using Application.DataTransferObjects;
using FluentValidation;


namespace Application.Validations
{
    public class WeatherCreateValidator : AbstractValidator<WeatherCreateDto>
    {
        public WeatherCreateValidator()
        {
            RuleFor(x => x.LowTemparature).NotEmpty().NotNull().WithMessage("Temperature is required.");
            RuleFor(x => x.HighTemperature).NotEmpty().NotNull().WithMessage("Code is required.");
            RuleFor(x => x.Pressure).NotEmpty().NotNull().WithMessage("Pressure is required.");
            RuleFor(x => x.Humidity).NotEmpty().NotNull().WithMessage("Humidity is required.");

        }

       
    }
    public class WeatherUpdateValidator : AbstractValidator<WeatherUpdateDto>
    {
        public WeatherUpdateValidator()
        {
            RuleFor(x => x.LowTemparature).NotEmpty().NotNull().WithMessage("Temperature is required.");
            RuleFor(x => x.HighTemperature).NotEmpty().NotNull().WithMessage("Code is required.");
            RuleFor(x => x.Pressure).NotEmpty().NotNull().WithMessage("Pressure is required.");
            RuleFor(x => x.Humidity).NotEmpty().NotNull().WithMessage("Humidity is required.");
        }
    }
}