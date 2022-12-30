using Microsoft.AspNetCore.Mvc;
using Dapr;
namespace TestSub.Controllers;

[ApiController]
[Route("")]
public class TestSubController : ControllerBase
{
    private readonly ILogger<TestSubController> _logger;

    public TestSubController(ILogger<TestSubController> logger)
    {
        _logger = logger;
    }

    [Topic("pubsub", "onboarding")]
    [HttpPost("/onboarding")]
    public void Run([FromBody] object message)
    {
        
        Console.WriteLine(message);
        Console.WriteLine("hit!!!");
        _logger.LogInformation("hit!!!");
    }
   
}
 