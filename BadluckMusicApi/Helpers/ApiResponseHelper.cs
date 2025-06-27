using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using BadluckMusicApi.Models.Entities;

namespace BadluckMusicApi.Helpers
{
    public class ApiResponseHelper
    {
        public static IActionResult ValidationError(ModelStateDictionary modelState)
        {
            return new BadRequestObjectResult(new ApiResponse
            {
                Status = "error",
                Message = "Validation errors",
                Errors = modelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                )
            });
        }

        public static IActionResult ServerError(ILogger logger, Exception ex, string message)
        {
            logger.LogError(ex, message);
            return new ObjectResult(new ApiResponse
            {
                Status = "error",
                Message = message
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public static IActionResult Success(string message, object? data = null)
        {
            return new OkObjectResult(new ApiResponse
            {
                Status = "success",
                Message = message,
                Data = data
            });
        }

        public static IActionResult BadRequest(string message, object? data = null)
        {
            return new BadRequestObjectResult(new ApiResponse
            {
                Status = "error",
                Message = message,
                Data = data
            });
        }

        public static IActionResult NotFoundError(string message)
        {
            return new NotFoundObjectResult(new ApiResponse
            {
                Status = "error",
                Message = message
            });
        }
    }
}
