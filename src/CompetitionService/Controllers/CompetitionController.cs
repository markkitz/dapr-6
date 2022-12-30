using Microsoft.AspNetCore.Mvc;
using Competition.Models;

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
    public IEnumerable<CompetitionDoc> Get()
    {
        return Enumerable.Range(1, 5)
        .Select(index => new CompetitionDoc(Guid.NewGuid().ToString(), "Competition " + index))
        .ToArray();
    }
}
