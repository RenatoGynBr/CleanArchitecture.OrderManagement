using CleanArchitecture.OrderManagement.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.OrderManagement.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(
        this Result result,
        ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.NoContent();
        }

        return result.Error?.Code switch
        {
            "Order.NotFound" =>
                controller.NotFound(new
                {
                    code = result.Error.Code,
                    message = result.Error.Message
                }),

            "Order.InvalidStatus" =>
                controller.BadRequest(new
                {
                    code = result.Error.Code,
                    message = result.Error.Message
                }),

            _ =>
                controller.BadRequest(new
                {
                    code = result.Error?.Code ?? "General.Failure",
                    message = result.Error?.Message ?? "An unexpected error occurred."
                })
        };
    }

    public static IActionResult ToActionResult<T>(
        this Result<T> result,
        ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(result.Value);
        }

        return result.Error?.Code switch
        {
            "Order.NotFound" =>
                controller.NotFound(new
                {
                    code = result.Error.Code,
                    message = result.Error.Message
                }),

            "Order.InvalidStatus" =>
                controller.BadRequest(new
                {
                    code = result.Error.Code,
                    message = result.Error.Message
                }),

            _ =>
                controller.BadRequest(new
                {
                    code = result.Error?.Code ?? "General.Failure",
                    message = result.Error?.Message ?? "An unexpected error occurred."
                })
        };
    }
}