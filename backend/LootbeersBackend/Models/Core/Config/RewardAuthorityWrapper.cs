using Solnet.Wallet;

namespace LootbeersBackend.Models.Core.Config;

public class RewardAuthorityWrapper
{
    public Account RewardAuthority { get; set; } = null!;
}