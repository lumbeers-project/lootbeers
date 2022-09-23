using Solnet.Wallet;

namespace LootbeersBackend.Models.Core;

public class Ticket
{
    public Ticket(PublicKey lootboxAddress, PublicKey userAddress)
    {
        LootboxAddress = lootboxAddress;
        UserAddress = userAddress;
    }

    public PublicKey LootboxAddress { get; }

    public PublicKey UserAddress { get; }
}