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
public class GetOfferController : ControllerBase
{
    private readonly ILogger<GetOfferController> _logger;
    private readonly IMapper _mapper;
    private readonly IOfferRepository _offerRepository;
    private const string _pubSub = "pubsub";

    public GetOfferController(ILogger<GetOfferController> logger, IMapper mapper, IOfferRepository offerRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _offerRepository = offerRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var offerDoc = await _offerRepository.GetOfferStateAsync(id);
        if (offerDoc.Value == null)
        {
            return NotFound();
        }
        return new JsonResult(offerDoc);
    }

}
