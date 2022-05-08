using Microsoft.AspNetCore.Mvc;

namespace dotnetcore.Controllers;

[ApiController]
[Route("[controller]")]
public class TestSessionController : ControllerBase
{
    private readonly ILogger<TestSessionController> _logger;

    public TestSessionController(ILogger<TestSessionController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetInstanceId")]
    public string? GetInstanceId()
    {
        return Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
    }

    [HttpGet("GetSession")]
    public TestResponse? Get(string name)
    {
        if (HttpContext.Session.Keys.Contains(name))
        {
            return ReturnResponseWithInstanceId(HttpContext.Session.GetString(name));
        }
        
        return ReturnResponseWithInstanceId(null);
    }

    [HttpPut("UpdateSession")]
    public TestResponse? Update(string name, string value)
    {
        HttpContext.Session.SetString(name, value);

        return ReturnResponseWithInstanceId(HttpContext.Session.GetString(name));
    }

    [HttpGet("Logout")]
    public void Logout()
    {
        HttpContext.Session.Clear();
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }
    }

    private TestResponse ReturnResponseWithInstanceId(string? value)
    {
        var response = new TestResponse();
        response.InstanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
        response.Result = value; 
        return response;
    }

    public class TestResponse
    {
        public string? Result { get; set; }
        public string? InstanceId { get; set; }
    }
}


