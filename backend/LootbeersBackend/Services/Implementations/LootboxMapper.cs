using LootbeersBackend.Models.Dao;
using LootbeersBackend.Models.Dto;

namespace LootbeersBackend.Services.Implementations;

public class LootboxMapper
{
    public Lootbox MapLootboxFromDto(LootboxDto lootboxDto)
    {
        Lootbox lootbox = new()
        {
            SolanaLootboxAddress = lootboxDto.Address,
            Rewards = lootboxDto.Rewards.Select(MapRewardFromDto).ToList()
        };

        return lootbox;
    }

    public Reward MapRewardFromDto(RewardDto rewardDto)
    {
        Reward reward = rewardDto.RewardType switch
        {
            RewardType.None => new NoneReward(),
            RewardType.Token => new TokenReward
            {
                Amount = rewardDto.Amount!.Value,
                MintAddress = rewardDto.Mint!
            },
            RewardType.Sol => new SolReward
            {
                Amount = rewardDto.Amount!.Value
            },
            RewardType.Nft => new NftReward(),
            _ => throw new ArgumentOutOfRangeException(nameof(rewardDto))
        };

        reward.Weight = rewardDto.Weight;
        reward.FrontId = rewardDto.FrontId;
        return reward;
    }
}