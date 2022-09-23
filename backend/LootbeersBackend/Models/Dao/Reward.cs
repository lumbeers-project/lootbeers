using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LootbeersBackend.Models.Dao;

public enum RewardType
{
    None,
    Token,
    Sol,
    Nft
}

[Index(nameof(LootboxId), IsUnique = false)]
public abstract class Reward
{
    [Key]
    public int Id { get; set; }
    
    public int FrontId { get; set; }

    [Required]
    [ForeignKey(nameof(Lootbox))]
    public int LootboxId { get; set; }

    [Required]
    public Lootbox Lootbox { get; set; } = null!;

    [Required]
    public int Weight { get; set; }

    [Required]
    public abstract RewardType RewardType { get; set; }
}

// Reward types

public class NoneReward : Reward
{
    [Required]
    public override RewardType RewardType { get; set; } = RewardType.None;
}

public class TokenReward : Reward
{
    [Required]
    public override RewardType RewardType { get; set; } = RewardType.Token;

    [Required]
    public string MintAddress { get; set; } = null!;

    [Required]
    public ulong Amount { get; set; }
}

public class SolReward : Reward
{
    [Required]
    public override RewardType RewardType { get; set; } = RewardType.Sol;

    [Required]
    public ulong Amount { get; set; }
}

public class NftReward : Reward
{
    [Required]
    public override RewardType RewardType { get; set; } = RewardType.Nft;
}