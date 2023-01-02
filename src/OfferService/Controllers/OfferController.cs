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


    

    // private string GetOfferUpdatedMessage(string prefix, Offer offer)
    // {
    //     return $"{prefix} for {offer.FirstName} {offer.LastName}. " +
    //     $"Offer ID: {offer.Id} " +
    //     $"Offer Status: {offer.Status}";
    // }
}
