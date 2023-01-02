using System.Text.Json.Serialization;
using Onboarding.Models.Offer;

namespace OfferService.Models;

public record struct Offer 
{
    public string Id { get; init; }
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

    [JsonConverter(typeof(OfferStatusJsonConverter))]
    public OfferStatus Status { get; init; }


}
