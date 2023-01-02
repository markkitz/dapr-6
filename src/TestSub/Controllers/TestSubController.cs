using Microsoft.AspNetCore.Mvc;
using Dapr;
using Onboarding.Core.Offer.Events;

namespace TestSub.Controllers;

[ApiController]
[Route("")]
public class TestSubController : ControllerBase
{
    private readonly ILogger<TestSubController> _logger;

    public TestSubController(ILogger<TestSubController> logger)
    {
        _logger = logger;
    }

    [Topic("pubsub", "offer.new")]
    [HttpPost("/newoffer")]
    public void NewOffer([FromBody] OfferCreated offer)
    {       
        _logger.LogInformation($"Offer Created for {offer.FirstName} {offer.LastName}. " +
        $"Offer ID: {offer.Id}");
    }
   
}
 