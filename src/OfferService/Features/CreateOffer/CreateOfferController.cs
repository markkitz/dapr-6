using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferService.Helpers;
using OfferService.Repositories;
using Onboarding.EventPubs;
using Onboarding.Core.Offer.Events;
using OfferService.Models;
using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics;

namespace OfferService.Features.CreateOffer;

[ApiController]
[Route("")]
public class CreateOfferController : ControllerBase
{
    private readonly ILogger<CreateOfferController> _logger;
    private readonly IMapper _mapper;
    private readonly IOfferRepository _offerRepository;
    private readonly IEventPub<OfferCreated> _eventPub;
    private readonly IValidator<NewOffer> _validator;

    public CreateOfferController(ILogger<CreateOfferController> logger, IMapper mapper, IOfferRepository offerRepository,  IEventPub<OfferCreated> pub, IValidator<NewOffer> validator )
    {
        _logger = logger;
        _mapper = mapper;
        _offerRepository = offerRepository;
        _eventPub = pub;
        _validator = validator;
    }

    [HttpPost]
    [TestFilter("CreateOfferController.NewOffer")]
    public async Task<IActionResult> NewOffer([FromBody] NewOffer newOffer)
    {
        ValidationResult result = _validator.Validate(newOffer);  
        if(!result.IsValid)
        {
            return new BadRequestObjectResult(result.Errors);
        }
        Offer offer = _mapper.Map<Offer>(newOffer);
        OfferCreated offerCreated = _mapper.Map<OfferCreated>(offer);
        Activity.Current?.AddTag("OfferId", offer.Id);
        Activity.Current?.AddTag("CompetitionId", offer.CompetitionId);
        await _offerRepository.SaveOfferStateAsync(offer);
        await _eventPub.PublishEventAsync(offerCreated);
        _logger.LogInformation(OfferLogger.getMessage("New offer created", offer));
        return new OkObjectResult(offerCreated);
    }

}
