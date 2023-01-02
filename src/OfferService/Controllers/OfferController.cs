using AutoMapper;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OfferService.Models;
using OfferService.Repositories;
using Onboarding.Models.Offer;
using Onboarding.Models.Offer.Events;

namespace OfferService.Controllers;

[ApiController]
[Route("")]
public class OfferController : ControllerBase
{
    private readonly ILogger<OfferController> _logger;
    private readonly IMapper _mapper;
    private readonly IOfferRepository _offerRepository;
    private const string _pubSub = "pubsub";

    public OfferController(ILogger<OfferController> logger, IMapper mapper, IOfferRepository offerRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _offerRepository = offerRepository;
    }

    [HttpGet("{id}")]
    public ActionResult<Offer> Get([FromState("statestore", "id")] StateEntry<Offer> offerDoc)
    {
        if (offerDoc.Value == null)
        {
            return NotFound();
        }

        return offerDoc.Value;
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

    [HttpPut("/ManagerSignOffRequested/{id}")]
    public async Task<IActionResult> ManagerSignOffRequested(string id, [FromServices] DaprClient daprClient)
    {
        var state = await _offerRepository.GetOfferStateAsync(id);
        if(state == null)
        {
            return NotFound();
        }

        Offer updatedOffer = state.Value with { Status = OfferStatus.ManagerSignOffRequested };
        OfferUpdated eventData = _mapper.Map<OfferUpdated>(updatedOffer);
        await _offerRepository.SaveOfferStateAsync(updatedOffer);
        await daprClient.PublishEventAsync(_pubSub, Topics.OfferUpdated, eventData);
        _logger.LogInformation(GetOfferUpdatedMessage("ManagerSignOffRequested", updatedOffer));
        return new OkObjectResult(updatedOffer);
    }
    [HttpPut("/ManagerSignOff/{id}")]
    public async Task<IActionResult> UpdateOffer(string id, [FromBody] ManagerSignOff signOff, [FromServices] DaprClient daprClient)
    {
        var state = await _offerRepository.GetOfferStateAsync(id);
        if(state == null)
        {
            return NotFound();
        }
        OfferStatus status = signOff.Approved ? OfferStatus.AwaitingDocumentCreation : OfferStatus.ManagerNotApproved;
        Offer updatedOffer = state.Value with { Status = status };
        OfferUpdated eventData = _mapper.Map<OfferUpdated>(updatedOffer);
        await _offerRepository.SaveOfferStateAsync(updatedOffer);
        await daprClient.PublishEventAsync(_pubSub, Topics.OfferUpdated, eventData);
        _logger.LogInformation(GetOfferUpdatedMessage("ManagerSignOffRequested", updatedOffer));
        return new OkObjectResult(updatedOffer);
    }

    private string GetOfferUpdatedMessage(string prefix, Offer offer)
    {
        return $"{prefix} for {offer.FirstName} {offer.LastName}. " +
        $"Offer ID: {offer.Id} " +
        $"Offer Status: {offer.Status}";
    }
}
