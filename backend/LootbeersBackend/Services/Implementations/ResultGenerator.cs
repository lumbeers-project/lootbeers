using LootbeersBackend.Models.Dao;
using LootbeersBackend.Models.Dto;

namespace LootbeersBackend.Services.Implementations;

// public class ResultGeneratorException : Exception
// {
//     public ResultGeneratorException()
//     {
//     }
//
//     protected ResultGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context)
//     {
//     }
//
//     public ResultGeneratorException(string? message) : base(message)
//     {
//     }
//
//     public ResultGeneratorException(string? message, Exception? innerException) : base(message, innerException)
//     {
//     }
// }

public class ResultGenerator : IResultGenerator
{
    public ResultGenerator(Random random)
    {
        Random = random;
    }

    public Random Random { get; set; }

    public TicketRedeemResult GetResult(Lootbox lootbox)
    {
        if (!lootbox.Rewards.Any())
        {
            throw new ArgumentException("Lootbox should have at least one reward");
        }

        var rewardsOrdered = lootbox.Rewards.OrderBy(r => r.Id).ToList();
        var rewardsSum = rewardsOrdered.Sum(r => r.Weight);
        var rewardNumber = Random.Next(rewardsSum);

        var currentSum = 0;

        foreach (Reward reward in rewardsOrdered)
        {
            currentSum += reward.Weight;

            if (currentSum > rewardNumber)
            {
                return RewardToRedeemResult(reward);
            }
        }

        return RewardToRedeemResult(rewardsOrdered.Last());
    }

    private static TicketRedeemResult RewardToRedeemResult(Reward reward)
    {
        switch (reward.RewardType)
        {
            case RewardType.None:
                return new TicketNoneRedeemResult
                {
                    Id = reward.FrontId
                };

            case RewardType.Token:
                TokenReward tokenReward = reward as TokenReward ??
                                          throw new NullReferenceException("Reward was expected to be of type TokenReward");

                return new TicketTokenRedeemResult
                {
                    Mint = tokenReward.MintAddress,
                    Amount = (long) tokenReward.Amount,
                    Id = tokenReward.FrontId
                };

            case RewardType.Sol:
                SolReward solReward = reward as SolReward ??
                                      throw new NullReferenceException("Reward was expected to be of type SolReward");

                return new TicketSolRedeemResult
                {
                    Amount = (long) solReward.Amount,
                    Id = solReward.FrontId
                };

            case RewardType.Nft:
                // NftReward nftReward = reward as NftReward ??
                //                       throw new NullReferenceException("Reward was expected to be of type NftReward");

                return new TicketNftRedeemResult
                {
                    Mint = "ADDRESS_TO_BE_ACCESSED",
                    Id = reward.FrontId
                };

            default:
                throw new ArgumentOutOfRangeException(nameof(reward));
        }
    }
}