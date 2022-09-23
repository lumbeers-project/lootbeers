using LootbeersBackend.Actions;
using LootbeersBackend.Models.Dto;
using LootbeersBackend.Validation;
using Microsoft.AspNetCore.Mvc;

namespace LootbeersBackend.Controllers;

[ApiController]
[Route("api/v1/ticket")]
public class TicketController : ControllerBase
{
    public TicketController(TicketActions actions)
    {
        Actions = actions;
    }

    public TicketActions Actions { get; }

    [TicketRedeemer]
    [HttpPost("redeem")]
    public async Task<TicketRedeemResult> RedeemTicket([FromBody] TicketRedeemRequest request)
    {
        return await Actions.RedeemTicket(new(request.TicketAddress));
    }
}