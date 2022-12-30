namespace OfferService.Events;

public record struct NewOffer
{
    public string CandidateId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}