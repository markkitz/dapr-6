using OfferService.Models;
using Dapr.Client;
namespace OfferService.Repositories;

public class DaprOfferRepository : IOfferRepository
{
    private const string DAPR_STORE_NAME = "statestore";
    private readonly DaprClient _daprClient;

    public DaprOfferRepository(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task SaveOfferStateAsync(Offer offer)
    {
        await _daprClient.SaveStateAsync<Offer>(
            DAPR_STORE_NAME, offer.Id, offer);
    }

    public async Task<Offer?> GetOfferStateAsync(string id)
    {
        var stateEntry = await _daprClient.GetStateEntryAsync<Offer>(
            DAPR_STORE_NAME, id);
        return stateEntry.Value;
    }

    public async Task<IReadOnlyList<Dapr.Client.StateQueryItem<OfferService.Models.Offer>>> GetOffersAsync()
    {
        var query = "{" +
                    "\"filter\": {" +
                       
                    "}}";
        var queryResponse = await _daprClient
            .QueryStateAsync<Offer>(DAPR_STORE_NAME, query);
        
        return queryResponse.Results;
    }
}
