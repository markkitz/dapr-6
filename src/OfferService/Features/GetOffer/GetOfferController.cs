
using Microsoft.AspNetCore.Mvc;
using OfferService.Repositories;

namespace OfferService.Controllers;

[ApiController]
[Route("")]
public class GetOfferController : ControllerBase
{
    private readonly ILogger<GetOfferController> _logger;
    private readonly IOfferRepository _offerRepository;

    public GetOfferController(ILogger<GetOfferController> logger,  IOfferRepository offerRepository)
    {
        _logger = logger;
        _offerRepository = offerRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var offerDoc = await _offerRepository.GetOfferStateAsync(id);
        if (offerDoc.Value == null)
        {
            _logger.LogWarning($"Offer not found: {id}", id);
            return NotFound();
        }
        return new JsonResult(offerDoc);
    }

}
