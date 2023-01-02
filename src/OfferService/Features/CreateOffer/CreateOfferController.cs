using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfferService.Helpers;
using OfferService.Repositories;
using Onboarding.EventPubs;
using Onboarding.Core.Offer.Events;
using OfferService.Models;

namespace OfferService.Features.CreateOffer;

[ApiController]
[Route("")]
public class CreateOfferController : ControllerBase
{
    private readonly ILogger<CreateOfferController> _logger;
    private readonly IMapper _mapper;
    private readonly IOfferRepository _offerRepository;
    private readonly IEventPub<OfferCreated> _eventPub;

    public CreateOfferController(ILogger<CreateOfferController> logger, IMapper mapper, IOfferRepository offerRepository,  IEventPub<OfferCreated> pub )
    {
        _logger = logger;
        _mapper = mapper;
        _offerRepository = offerRepository;
        _eventPub = pub;
    }

    [HttpPost]
    public async Task<IActionResult> NewOffer([FromBody] NewOffer newOffer)
    {
        Offer offer = _mapper.Map<Offer>(newOffer);
        OfferCreated offerCreated = _mapper.Map<OfferCreated>(offer);
        await _offerRepository.SaveOfferStateAsync(offer);
        await _eventPub.PublishEventAsync(offerCreated);
        _logger.LogInformation(OfferLogger.getMessage("New offer created", offer));
        return new OkObjectResult(offerCreated);
    }

}
