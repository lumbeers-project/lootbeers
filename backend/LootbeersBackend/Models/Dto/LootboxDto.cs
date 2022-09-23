using System.ComponentModel.DataAnnotations;

namespace LootbeersBackend.Models.Dto;

public class LootboxDto
{
    [Required]
    public string Address { get; set; } = null!;

    [Required]
    public ICollection<RewardDto> Rewards { get; set; } = null!;
}