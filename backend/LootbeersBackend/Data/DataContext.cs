using LootbeersBackend.Models.Dao;
using Microsoft.EntityFrameworkCore;

namespace LootbeersBackend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Lootbox> Lootboxes { get; set; } = null!;

    public DbSet<Reward> Rewards { get; set; } = null!;

    public DbSet<NoneReward> NoneRewards { get; set; } = null!;

    public DbSet<TokenReward> TokenRewards { get; set; } = null!;

    public DbSet<SolReward> SolRewards { get; set; } = null!;

    public DbSet<NftReward> NftRewards { get; set; } = null!;
}