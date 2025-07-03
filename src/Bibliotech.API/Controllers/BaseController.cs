using Bibliotech.Core.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotech.API.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;

    protected BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
            return NotFound(new { Error = result.Error });

        return BadRequest(new { Errors = result.Errors });
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return Ok();

        return BadRequest(new { Errors = result.Errors });
    }
}