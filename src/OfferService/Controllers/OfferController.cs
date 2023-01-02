using AutoMapper;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OfferService.Models;
using Onboarding.Models.Offer;
using Onboarding.Models.Offer.Events;

namespace OfferService.Controllers;

[ApiController]
[Route("")]
public class OfferController : ControllerBase
{
    private readonly ILogger<OfferController> _logger;
    private readonly IMapper _mapper;

    public OfferController(ILogger<OfferController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
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
        await daprClient.SaveStateAsync("statestore", offer.Id, offer);
        await daprClient.PublishEventAsync("pubsub", Topics.OfferNew, offerCreated);
        _logger.LogInformation(GetOfferUpdatedMessage("New offer created", offer));
        return new OkObjectResult(offerCreated);
    }

    [HttpPut("/ManagerSignOffRequested/{id}")]
    public async Task<IActionResult> ManagerSignOffRequested([FromState("statestore", "id")] StateEntry<Offer> keyValue, [FromServices] DaprClient daprClient)
    {
        Offer updatedOffer = keyValue.Value with { Status = OfferStatus.ManagerSignOffRequested };
        await daprClient.SaveStateAsync("statestore", updatedOffer.Id, updatedOffer);
        await daprClient.PublishEventAsync("pubsub", Topics.OfferUpdated, updatedOffer);
        _logger.LogInformation(GetOfferUpdatedMessage("ManagerSignOffRequested", updatedOffer));
        return new OkObjectResult(updatedOffer);
    }
    [HttpPut("/ManagerSignOff/{id}")]
    public async Task<IActionResult> UpdateOffer([FromBody] ManagerSignOff signOff, [FromState("statestore", "id")] StateEntry<Offer> keyValue, [FromServices] DaprClient daprClient)
    {
        OfferStatus status = signOff.Approved ? OfferStatus.AwaitingDocumentCreation : OfferStatus.ManagerNotApproved;
        Offer updatedOffer = keyValue.Value with { Status = status };
        await daprClient.SaveStateAsync("statestore", updatedOffer.Id, updatedOffer);
        await daprClient.PublishEventAsync("pubsub", Topics.OfferUpdated, updatedOffer);
        return new OkObjectResult(updatedOffer);
    }

    private string GetOfferUpdatedMessage(string prefix, Offer offer)
    {
        return $"{prefix} for {offer.FirstName} {offer.LastName}. " +
        $"Offer ID: {offer.Id} " +
        $"Offer Status: {offer.Status}";
    }
}
