namespace Application.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(256) // Identity default max length
                .Matches("^[a-zA-Z0-9._-]+$")
                .WithMessage("Username can only contain letters, numbers, dots, underscores, and hyphens");

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(256) // Identity default max length
                .EmailAddress()
                .WithMessage("A valid email address is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100)
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
                .When(x => string.IsNullOrEmpty(x.Id) || !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Phone number must be in a valid international format");
        }
    }
}
