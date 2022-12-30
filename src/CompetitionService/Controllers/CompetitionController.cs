using Microsoft.AspNetCore.Mvc;

namespace CompetitionService.Controllers;

[ApiController]
[Route("")]
public class CompetitionController : ControllerBase
{

    private readonly ILogger<CompetitionController> _logger;

    public CompetitionController(ILogger<CompetitionController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Competition> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Competition
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Competition " + index
        })
        .ToArray();
    }
}
