using System.ComponentModel.DataAnnotations;
using LootbeersBackend.Models.Dao;

namespace LootbeersBackend.Models.Dto;

public class RewardDto
{
    [Required]
    public RewardType RewardType { get; set; }

    [Required]
    public int FrontId { get; set; }

    [Required]
    public int Weight { get; set; }

    public string? Mint { get; set; }

    public ulong? Amount { get; set; }
}