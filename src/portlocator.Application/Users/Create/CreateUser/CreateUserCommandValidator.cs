using FluentValidation;

namespace portlocator.Application.Users.Create.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(200)
                .WithMessage("User name should not be empty and not longer than 200 characters");
            RuleFor(x => x.RoleId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Role is required.");
        }
    }
}
