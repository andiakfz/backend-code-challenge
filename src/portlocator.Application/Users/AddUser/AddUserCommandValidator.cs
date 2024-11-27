using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Users.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
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
