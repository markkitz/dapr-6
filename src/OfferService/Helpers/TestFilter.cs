using Microsoft.AspNetCore.Mvc.Filters;

public class TestFilterAttribute : Attribute, IActionFilter
{
    private readonly string name;

    public TestFilterAttribute(string name)
    {
        Console.WriteLine("TestFilter");
        this.name = name;
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"TestFilter.OnActionExecuting: {name}" );
        // Do something before the action executes.
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do something after the action executes
        Console.WriteLine($"TestFilter.OnActionExecuted: {name}");
    }
}