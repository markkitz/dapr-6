using System.Text.Json.Serialization;
using Onboarding.Core.Offer;

namespace Onboarding.Core.Offer.Events;

public record struct OfferCreated
{
    public string Id { get; init; }
    public string CandidateId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PersonalEmail {get;init;}
    public string Manager { get; init; }
    public string CompetitionId { get; init; }
    public string PositionName { get; init; }
    public string Level { get; init; }
    public int Step { get; init; }
    public double Salary { get; init; }
    [JsonConverter(typeof(OfferStatusJsonConverter))]
    public OfferStatus Status { get; init; }

}
