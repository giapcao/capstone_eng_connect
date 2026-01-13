using EngConnect.BuildingBlock.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.BuildingBlock.Presentation.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult FromResult<T>(Result<T> result)
    {
        // Always respect the HttpStatusCode carried by Result
        return StatusCode((int)result.HttpStatusCode, result);
    }

    protected IActionResult FromResult(Result result)
    {
        // Always respect the HttpStatusCode carried by Result
        return StatusCode((int)result.HttpStatusCode, result);
    }

    protected IActionResult FromResultWithFailureValue<T>(Result<T> result)
    {
        // Preserve custom shape including failure value, but use HttpStatusCode
        return StatusCode((int)result.HttpStatusCode, new
        {
            isSuccess = result.IsSuccess,
            data = result.Value,
            error = result.Error is null
                ? null
                : new
                {
                    code = result.Error.Code,
                    message = result.Error.Message
                },
            failureValue = result.FailureValue
        });
    }
}