namespace OfferService.Features.CreateOffer;
using FluentValidation;

public record struct NewOffer
{
    public string CandidateId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string personalEmail {get;init;}
    public string manager { get; init; }
    public string competitionId { get; init; }
    public string positionName { get; init; }
    public string level { get; init; }
    public int step { get; init; }
    public double salary { get; init; }
}

public class NewOfferValidator : AbstractValidator<NewOffer>
{
    public NewOfferValidator()
    {
        RuleFor(x => x.CandidateId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().Matches(@"^[a-zA-Z]+$");
        RuleFor(x => x.LastName).NotEmpty().Matches(@"^[a-zA-Z]+$");
        RuleFor(x => x.personalEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.manager).NotEmpty();
        RuleFor(x => x.competitionId).NotEmpty();
        RuleFor(x => x.positionName).NotEmpty();
        RuleFor(x => x.level).NotEmpty();
        RuleFor(x => x.step).GreaterThan(0).WithMessage("Step must be greater than 0");
        RuleFor(x => x.salary).GreaterThanOrEqualTo(0).WithMessage("Salary must be greater than or equal to 0");
    }
}