using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace AwningsAPI.Functions;

/// <summary>
/// Single catch-all HTTP trigger — the ASP.NET Core pipeline (controllers,
/// middleware, JWT auth) handles all routing. This class is just the entry
/// point required by the Functions runtime.
/// </summary>
public class HttpTrigger
{
    [Function("HttpTrigger")]
    public Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, Route = "{**catch-all}")]
        HttpRequest req)
    {
        // Never reached — ConfigureFunctionsWebApplication() intercepts all
        // HTTP requests and forwards them to the ASP.NET Core pipeline.
        return Task.FromResult<IActionResult>(new OkResult());
    }
}
