using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Ships.Create
{
    public class CreateShipCommandValidator : AbstractValidator<CreateShipCommand>
    {
        public CreateShipCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Latitude)
                // ADD PRECISION
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 degrees and 90 degrees.");

            RuleFor(x => x.Longitude)
                // ADD PRECISION
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 degrees and 180 degrees.");

            RuleFor(x => x.Velocity)
                // ADD PRECISION
                .GreaterThanOrEqualTo(0).WithMessage("Velocity must start from zero.");
        }
    }
}
