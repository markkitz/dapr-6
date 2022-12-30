using System.Text.Json.Serialization;
using OfferService.Events;
using Onboarding.Models.Offer;

namespace OfferService.Models;

public record struct OfferDoc 
{
    public string Id { get; init; }
    public string CandidateId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    [JsonConverter(typeof(OfferStatusJsonConverter))]
    public OfferStatus Status { get; init; }


    public OfferDoc(string id, NewOffer newOffer){
        Id = id;
        CandidateId = newOffer.CandidateId;
        FirstName = newOffer.FirstName;
        LastName = newOffer.LastName;
        Status = OfferStatus.New;
    }

}
