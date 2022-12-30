using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OfferService.Events;
using OfferService.Models;

namespace OfferService.Controllers;

[ApiController]
[Route("")]
public class OfferController : ControllerBase
{
    // private static readonly string[] Summaries = new[]
    // {
    //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    // };

    private readonly ILogger<OfferController> _logger;

    public OfferController(ILogger<OfferController> logger)
    {
        _logger = logger;
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<WeatherForecast> Get([FromServices] DaprClient daprClient)
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //     {
    //         Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    // }

    [HttpGet("{id}")]
    public ActionResult<OfferDoc> Get([FromState("statestore", "id")] StateEntry<OfferDoc> offerDoc)
    {
        if (offerDoc.Value == null)
        {
        return NotFound();
        }

        return offerDoc.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] NewOffer newOffer, [FromServices] DaprClient daprClient)//, [FromState("statestore", "city")] StateEntry<WeatherForecast> stateEntry)
    {
        _logger.LogInformation("hit!!! Post");
        string id = Guid.NewGuid().ToString();
        OfferDoc offerDoc = new OfferDoc(id, newOffer);
        await daprClient.SaveStateAsync("statestore", offerDoc.Id, offerDoc);     
        await daprClient.PublishEventAsync("pubsub", "onboarding", offerDoc);   
        return new OkObjectResult(offerDoc);
    }

}
