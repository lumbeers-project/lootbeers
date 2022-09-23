using LootbeersBackend.Actions;
using LootbeersBackend.Models.Core.Config;
using LootbeersBackend.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace LootbeersBackend.Controllers;

[ApiController]
[Route("api/v1/lootbox")]
public class LootboxController : ControllerBase
{
    public LootboxController(LootboxActions lootboxActions, AdminTokenWrapper adminTokenWrapper)
    {
        LootboxActions = lootboxActions;
        AdminTokenWrapper = adminTokenWrapper;
    }

    public LootboxActions LootboxActions { get; set; }

    public AdminTokenWrapper AdminTokenWrapper { get; }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateLootbox([FromBody] LootboxDto lootbox)
    {
        try
        {
            if (!CheckToken())
            {
                return Forbid();
            }

            await LootboxActions.AddLootbox(lootbox);
            return Ok();
        }
        catch (LootboxAddressAlreadyExists)
        {
            return BadRequest(new ProblemDetails
            {
                Detail = "lootboxAddressAlreadyExists"
            });
        }
    }

    private bool CheckToken()
    {
        if (!HttpContext.Request.Headers.TryGetValue("TOKEN", out StringValues tokenHeader))
        {
            return false;
        }

        var tokenValue = tokenHeader.FirstOrDefault();

        return tokenValue == AdminTokenWrapper.AccessToken;
    }
}