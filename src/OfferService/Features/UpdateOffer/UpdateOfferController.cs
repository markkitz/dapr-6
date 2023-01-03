using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferService.Helpers;
using OfferService.Models;
using OfferService.Repositories;
using Onboarding.EventPubs;
using Onboarding.Core.Offer;
using Onboarding.Core.Offer.Events;
using FluentValidation;
using FluentValidation.Results;

namespace OfferService.Features.UpdateOffer;

[ApiController]
[Route("")]
public class UpdateOfferController : ControllerBase
{
    private readonly ILogger<UpdateOfferController> _logger;
    private readonly IMapper _mapper;
    private readonly IOfferRepository _offerRepository;
    private readonly IEventPub<OfferUpdated> _eventPub;
    private readonly IValidator<ManagerSignOff> _managerSignoffValidator;

    public UpdateOfferController(ILogger<UpdateOfferController> logger, IMapper mapper, 
    IOfferRepository offerRepository, IEventPub<OfferUpdated> eventPub, IValidator<ManagerSignOff> managerSignoffValidator)
    {
        _logger = logger;
        _mapper = mapper;
        _offerRepository = offerRepository;
        _eventPub = eventPub;
        _managerSignoffValidator = managerSignoffValidator;
    }

    [HttpPut("/ManagerSignOffRequested/{id}")]
    public async Task<IActionResult> ManagerSignOffRequested(string id)
    {
        var state = await _offerRepository.GetOfferStateAsync(id);
        if (state == null)
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
    public async Task<IActionResult> UpdateOffer(string id, [FromBody] ManagerSignOff signOff)
    {
        ValidationResult result = _managerSignoffValidator.Validate(signOff);
        if (!result.IsValid || !signOff.Approved.HasValue )
        {
            return new BadRequestObjectResult(result.Errors );
        }
        var state = await _offerRepository.GetOfferStateAsync(id);
        if (state == null)
        {
            return NotFound();
        }
        OfferStatus status = signOff.Approved.Value ? OfferStatus.AwaitingDocumentCreation : OfferStatus.ManagerNotApproved;
        Offer updatedOffer = state.Value with { Status = status };
        OfferUpdated eventData = _mapper.Map<OfferUpdated>(updatedOffer);
        await _offerRepository.SaveOfferStateAsync(updatedOffer);
        await _eventPub.PublishEventAsync(eventData);
        _logger.LogInformation(OfferLogger.getMessage("Manager SignOff", updatedOffer, $"Approved: {signOff.Approved} Manager: {updatedOffer.manager}"));
        return new OkObjectResult(updatedOffer);
    }

}
