using Microsoft.AspNetCore.Mvc;
using Dapr;
namespace TestSub.Controllers;

[ApiController]
[Route("")]
public class TestSub : ControllerBase
{
    private readonly ILogger<CollectionController> _logger;

    public CollectionController(ILogger<CollectionController> logger)
    {
        _logger = logger;
    }

    [Topic("pubsub", "onboarding")]
    [HttpPost]
    public void Run([FromBody] object message)
    {
        
        Console.WriteLine(message);
    }
   
}
 