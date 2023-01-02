using AutoMapper;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OfferService.Models;
using OfferService.Repositories;
using Onboarding.Models.Offer;
using Onboarding.Models.Offer.Events;

namespace OfferService.Features.CreateOffer;

[ApiController]
[Route("")]
public class CreateOfferController : ControllerBase
{
    private readonly ILogger<CreateOfferController> _logger;
    private readonly IMapper _mapper;
    private readonly IOfferRepository _offerRepository;
    private const string _pubSub = "pubsub";

    public CreateOfferController(ILogger<CreateOfferController> logger, IMapper mapper, IOfferRepository offerRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _offerRepository = offerRepository;
    }


    [HttpPost]
    public async Task<IActionResult> NewOffer([FromBody] NewOffer newOffer, [FromServices] DaprClient daprClient)
    {
        Offer offer = _mapper.Map<Offer>(newOffer);
        OfferCreated offerCreated = _mapper.Map<OfferCreated>(offer);
        await _offerRepository.SaveOfferStateAsync(offer);
        await daprClient.PublishEventAsync("pubsub", Topics.OfferNew, offerCreated);
        _logger.LogInformation(GetOfferUpdatedMessage("New offer created", offer));
        return new OkObjectResult(offerCreated);
    }


    private string GetOfferUpdatedMessage(string prefix, Offer offer)
    {
        return $"{prefix} for {offer.FirstName} {offer.LastName}. " +
        $"Offer ID: {offer.Id} " +
        $"Offer Status: {offer.Status}";
    }
}
