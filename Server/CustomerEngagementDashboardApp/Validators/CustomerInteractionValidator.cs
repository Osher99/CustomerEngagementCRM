using CustomerEngagementDashboardApp.Dtos;
using FluentValidation;
using static CustomerEngagementDashboardApp.Constants.Enums;

namespace CustomerEngagementDashboardApp.Validators
{
    public class CustomerInteractionValidator : AbstractValidator<CustomerInteractionDTO>
    {
        public CustomerInteractionValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name must be less than 100 characters.");

            RuleFor(x => x.InteractionType)
                .NotEmpty().WithMessage("Interaction type is required.")
                .Must(x => Enum.IsDefined(typeof(InteractionType), x))
                .WithMessage("Invalid interaction type.");

            RuleFor(x => x.InteractionDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Interaction date cannot be in the future.");

            RuleFor(x => x.Outcome)
                .NotEmpty().WithMessage("Outcome is required.");
        }
    }
}
