using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ships.Update.UpdateVelocity
{
    public class UpdateShipVelocityCommandValidator : AbstractValidator<UpdateShipVelocityCommand>
    {
        public UpdateShipVelocityCommandValidator()
        {
            RuleFor(x => x.ShipId)
                .NotEmpty().WithMessage("Ship Id must be a valid guid value");

            RuleFor(x => x.Velocity)
                .GreaterThanOrEqualTo(0).WithMessage("Velocity must start from zero.");
        }
    }
}
