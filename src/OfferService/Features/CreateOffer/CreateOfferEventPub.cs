using Dapr.Client;
using Onboarding.EventPubs;
using Onboarding.Core.Offer;
using Onboarding.Core.Offer.Events;

namespace OfferService.Features.CreateOffer;

public class CreateOfferEventPub : IEventPub<OfferCreated>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<CreateOfferEventPub> _logger;

    public CreateOfferEventPub(DaprClient daprClient, ILogger<CreateOfferEventPub> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task PublishEventAsync(OfferCreated eventData)
    {
        if(eventData == null){
            throw new ArgumentNullException(nameof(eventData));
        }
        await _daprClient.PublishEventAsync("pubsub", Topics.OfferNew, eventData);
        _logger.LogInformation($"Published event {eventData.GetType().Name}");
    }
}