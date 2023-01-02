using OfferService.EventPubs;

namespace OfferService.Features.CreateOffer;

public class CreateOfferEventPub : IEventPub
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<CreateOfferEventPub> _logger;

    public CreateOfferEventPub(DaprClient daprClient, ILogger<CreateOfferEventPub> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task PublishEventAsync<T>(T eventData)
    {
        await _daprClient.PublishEventAsync("pubsub", Topics.OfferNew, eventData);
        _logger.LogInformation($"Published event {eventData.GetType().Name}");
    }
}