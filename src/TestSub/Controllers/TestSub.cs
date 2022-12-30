using Microsoft.AspNetCore.Mvc;
using Dapr;
namespace TestSub.Controllers;

[ApiController]
[Route("")]
public class TestSub : ControllerBase
{

    [Topic("pubsub", "onboarding")]
    [HttpPost("/Process")]
    public void Run([FromBody] object message)
    {
        Console.WriteLine(message);
    }
   
}
