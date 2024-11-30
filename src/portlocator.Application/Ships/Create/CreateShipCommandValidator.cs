using FluentValidation;

namespace portlocator.Application.Ships.Create
{
    public class CreateShipCommandValidator : AbstractValidator<CreateShipCommand>
    {
        public CreateShipCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 degrees and 90 degrees.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 degrees and 180 degrees.");

            RuleFor(x => x.Velocity)
                .GreaterThanOrEqualTo(0).WithMessage("Velocity must be greater than or equal 0.");
        }
    }
}
