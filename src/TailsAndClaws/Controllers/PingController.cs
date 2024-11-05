using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using TailsAndClaws.Common.Constants;
using TailsAndClaws.Common.Options;

namespace TailsAndClaws.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting(ApiConstants.DefaultRateLimitingPolicyName)]
[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
public class PingController(IOptions<PingEndpointOptions> options) : ControllerBase
{
    private readonly IOptions<PingEndpointOptions> _options = options;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public Task<IActionResult> Ping(CancellationToken cancellationToken = default)
    {
        return Task
            .FromResult<IActionResult>(
                Ok(_options.Value.ReturnMessage));
    }
}
