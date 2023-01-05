
using Microsoft.AspNetCore.Mvc;
using OfferService.Repositories;

namespace OfferService.Controllers;

[ApiController]
[Route("")]
public class GetOffersController : ControllerBase
{
    private readonly ILogger<GetOffersController> _logger;
    private readonly IOfferRepository _offerRepository;

    public GetOffersController(ILogger<GetOffersController> logger,  IOfferRepository offerRepository)
    {
        _logger = logger;
        _offerRepository = offerRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var offers = await _offerRepository.GetOffersAsync();

        return new JsonResult(offers);
    }

}
