using Solnet.Wallet;

namespace LootbeersBackend.Models.Core.Config;

public class RootConfigIdWrapper
{
    public PublicKey RootConfigAddress { get; init; } = null!;
}