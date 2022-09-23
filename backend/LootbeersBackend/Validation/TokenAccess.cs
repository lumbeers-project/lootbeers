// using LootbeersBackend.Models.Core.Config;
// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Microsoft.Extensions.Primitives;
//
// namespace LootbeersBackend.Validation;
//
// [AttributeUsage(AttributeTargets.Method)]
// public class TokenAccess : IAsyncAuthorizationFilter
// {
//     public Task OnAuthorizationAsync(AuthorizationFilterContext context)
//     {
//         AdminTokenWrapper adminToken = context.HttpContext.RequestServices.GetRequiredService<AdminTokenWrapper>();
//         if (!context.HttpContext.Request.Headers.TryGetValue("TOKEN", out StringValues tokenHeader))
//         {
//             context.Result = new ForbidResult();
//         }
//
//         var tokenValue = tokenHeader.FirstOrDefault();
//
//         if (tokenValue != adminToken.AccessToken)
//         {
//             context.Result = new ForbidResult();
//         }
//         
//         return Task.CompletedTask;
//     }
// }

