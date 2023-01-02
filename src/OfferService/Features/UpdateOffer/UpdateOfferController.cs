using AutoMapper;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OfferService.Helpers;
using OfferService.Models;
using OfferService.Repositories;
using Onboarding.EventPubs;
using Onboarding.Models.Offer;
using Onboarding.Models.Offer.Events;

namespace OfferService.Features.CreateOffer;

[ApiController]
[Route("")]
public class UpdateOfferController : ControllerBase
{
    private readonly ILogger<UpdateOfferController> _logger;
    private readonly IMapper _mapper;
    private readonly IOfferRepository _offerRepository;
    private readonly IEventPub<OfferUpdated> _eventPub;


    public UpdateOfferController(ILogger<UpdateOfferController> logger, IMapper mapper, IOfferRepository offerRepository, IEventPub<OfferUpdated> eventPub)
    {
        _logger = logger;
        _mapper = mapper;
        _offerRepository = offerRepository;
        _eventPub = eventPub;
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
        await _eventPub.PublishEventAsync(eventData);
         _logger.LogInformation(OfferLogger.getMessage("Manager SignOff Requested", updatedOffer, $"Manager: {updatedOffer.manager}"));
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
        await _eventPub.PublishEventAsync(eventData);
        _logger.LogInformation(OfferLogger.getMessage("Manager SignOff", updatedOffer, $"Approved: {signOff.Approved} Manager: {updatedOffer.manager}"));
        return new OkObjectResult(updatedOffer);
    }



}
