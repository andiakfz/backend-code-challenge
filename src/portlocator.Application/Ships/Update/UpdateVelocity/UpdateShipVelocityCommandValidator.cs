using FluentValidation;

namespace portlocator.Application.Ships.Update.UpdateVelocity
{
    public class UpdateShipVelocityCommandValidator : AbstractValidator<UpdateShipVelocityCommand>
    {
        public UpdateShipVelocityCommandValidator()
        {
            RuleFor(x => x.ShipId)
                .NotEmpty().WithMessage("Ship Id must be a valid guid value");

            RuleFor(x => x.Velocity)
                .GreaterThanOrEqualTo(0).WithMessage("Velocity must be greater than or equal 0.");
        }
    }
}
