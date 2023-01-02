using Dapr.Client;
using Onboarding.EventPubs;
using Onboarding.Core.Offer;
using Onboarding.Core.Offer.Events;

namespace OfferService.Features.UpdateOffer;

public class UpdateOfferEventPub : IEventPub<OfferUpdated>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<UpdateOfferEventPub> _logger;

    public UpdateOfferEventPub(DaprClient daprClient, ILogger<UpdateOfferEventPub> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task PublishEventAsync(OfferUpdated eventData)
    {
        // if(eventData == null){
        //     throw new ArgumentNullException(nameof(eventData));
        // }
        await _daprClient.PublishEventAsync("pubsub", Topics.OfferUpdated, eventData);
        _logger.LogInformation($"Published event {eventData.GetType().Name}");
    }
}