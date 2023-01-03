using FluentValidation;

namespace OfferService.Features.UpdateOffer;

public record struct ManagerSignOff
{
    public bool Approved { get; init; }
}

public class ManagerSignOffValidator : AbstractValidator<ManagerSignOff>
{
    public ManagerSignOffValidator()
    {
        RuleFor(x => x.Approved).NotNull();
    }
}