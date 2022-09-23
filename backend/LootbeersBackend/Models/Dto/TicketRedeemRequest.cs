using System.ComponentModel.DataAnnotations;

namespace LootbeersBackend.Models.Dto;

public class TicketRedeemRequest
{
    [Required]
    public string TicketAddress { get; set; } = null!;
}