using LootbeersBackend.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Solnet.Wallet;

namespace LootbeersBackend.Validation;

[AttributeUsage(AttributeTargets.Method)]
public class TicketRedeemerAttribute : ActionFilterAttribute
{
    private static string ParameterName => "request";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.ContainsKey(ParameterName))
        {
            if (context.ActionArguments[ParameterName] is not TicketRedeemRequest request)
            {
                context.Result = new BadRequestObjectResult(new ProblemDetails
                {
                    Detail = $"The required parameter {ParameterName} is not found in the request"
                });
            }
            else
            {
                if (!PublicKey.IsValid(request.TicketAddress))
                {
                    context.Result = new BadRequestObjectResult(new ProblemDetails
                    {
                        Detail = $"{nameof(request.TicketAddress)} field is not a valid base58 address"
                    });
                }
            }
        }
    }
}