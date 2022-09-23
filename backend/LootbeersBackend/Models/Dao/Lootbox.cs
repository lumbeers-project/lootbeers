using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LootbeersBackend.Models.Dao;

[Index(nameof(SolanaLootboxAddress), IsUnique = true)]
public class Lootbox
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string SolanaLootboxAddress { get; set; } = null!;

    public ICollection<Reward> Rewards { get; set; } = null!;
}