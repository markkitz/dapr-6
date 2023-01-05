using OfferService.Models;
namespace OfferService.Repositories;

public interface IOfferRepository
{
    Task SaveOfferStateAsync(Offer offer);
    Task<Offer?> GetOfferStateAsync(string id);

    Task<IReadOnlyList<Dapr.Client.StateQueryItem<OfferService.Models.Offer>>> GetOffersAsync();
}
