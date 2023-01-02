using OfferService.Models;

namespace OfferService.Helpers;

public static  class OfferLogger
{
    public static string getMessage(string prefix, Offer offer, string suffix = ""){
        return $"{prefix} for {offer.FirstName} {offer.LastName}. " +
        $"Offer ID: {offer.Id} " +
        $"Offer Status: {offer.Status} " + suffix;
    }
}