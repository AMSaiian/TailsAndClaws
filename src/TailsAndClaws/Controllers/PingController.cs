﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TailsAndClaws.Common.Options;

namespace TailsAndClaws.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
public class PingController(IOptions<PingEndpointOptions> options) : ControllerBase
{
    private readonly IOptions<PingEndpointOptions> _options = options;

    [HttpGet]
    public Task<IActionResult> Ping(CancellationToken cancellationToken = default)
    {
        return Task
            .FromResult<IActionResult>(
                Ok(_options.Value.ReturnMessage));
    }
}