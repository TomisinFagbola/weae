using Application.DataTransferObjects;
using Domain.Enums;
using FluentValidation;


namespace Application.Validations
{
    public class UserValidator : AbstractValidator<UserCreateDTO>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First Name is required");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last Name is required");
            RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone Number is required");
        }
    }
    public class UserUpdateValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First Name is required");
            RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last Name is required");
            RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone Number is required");
         

        }
    }

    public class SetUserPassWordValidator : AbstractValidator<SetPasswordDTO>
    {
        public SetUserPassWordValidator()
        {
            RuleFor(x => x.Token).NotNull().NotEmpty().WithMessage("Token cannot be null or empty");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be null or empty");
            RuleFor(x => x.Password)
                .Matches(@"(?-i)(?=^.{8,}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))((?=(.*\d){1,})|(?=(.*\W){1,}))^.*$")
                .WithMessage("Password must be at least 8 characters, have at least 1 upper case letter (A – Z), 1 lower case letter (a – z), 1 number (0 – 9) and 1 non-alphanumeric symbol (e.g. @ '$%£! ')");
        }
    }

    public class UserLoginValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty().WithMessage("Email cannot be null or empty");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be null or empty");
        }
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty().WithMessage("Email cannot be null or empty");
        }
    }
}

