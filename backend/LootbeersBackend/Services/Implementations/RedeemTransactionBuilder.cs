using System.Text;
using LootbeersBackend.Models.Core;
using LootbeersBackend.Models.Core.Config;
using LootbeersBackend.Models.Dto;
using LootboxOpener.Program;
using LootboxOpener.Types;
using Microsoft.AspNetCore.Components;
using Solnet.Programs;
using Solnet.Rpc;
using Solnet.Rpc.Models;
using Solnet.Rpc.Types;
using Solnet.Wallet;

namespace LootbeersBackend.Services.Implementations;

public class RedeemTransactionBuilder : IRedeemTransactionBuilder
{
    public RedeemTransactionBuilder(IRpcClient rpcClient, LootboxProgramIdWrapper lootboxProgramId, RewardAuthorityWrapper rewardAuthority, RootConfigIdWrapper rootConfigId)
    {
        RpcClient = rpcClient;
        LootboxProgramId = lootboxProgramId;
        RewardAuthority = rewardAuthority;
        RootConfigId = rootConfigId;
    }

    public IRpcClient RpcClient { get; set; }

    public LootboxProgramIdWrapper LootboxProgramId { get; set; }

    public RewardAuthorityWrapper RewardAuthority { get; set; }

    public RootConfigIdWrapper RootConfigId { get; set; }

    public Transaction GetRedeemTransaction(TicketRedeemResult redeemResult, Ticket ticket, PublicKey ticketAddress)
    {
        (PayoutReward reward, PublicKey? mintKey) = CreatePayoutReward(redeemResult);

        PublicKey.TryFindProgramAddress(new[]
            {
                Encoding.UTF8.GetBytes("treasure_chest"),
                RootConfigId.RootConfigAddress.KeyBytes
            },
            LootboxProgramId.ProgramId,
            out PublicKey treasureChest,
            out _);

        PublicKey fallbackMint = new("So11111111111111111111111111111111111111112");
        PublicKey sourceToken = mintKey is null
            ? AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(treasureChest, fallbackMint)
            : AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(treasureChest, mintKey);

        PublicKey destinationToken = mintKey is null
            ? AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(ticket.UserAddress, fallbackMint)
            : AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(ticket.UserAddress, mintKey);

        // Generate transaction for reward claim and token account initialization
        TransactionInstruction redeemIx = LootboxOpenerProgram.RedeemTicket(
            new()
            {
                RootConfig = RootConfigId.RootConfigAddress,
                Lootbox = ticket.LootboxAddress,
                User = ticket.UserAddress,
                Ticket = ticketAddress,
                RewardAuthority = RewardAuthority.RewardAuthority.PublicKey,
                SourceToken = sourceToken,
                UserToken = destinationToken,
                TreasureChest = treasureChest,
                SystemProgram = SystemProgram.ProgramIdKey,
                TokenProgram = TokenProgram.ProgramIdKey
            },
            reward,
            LootboxProgramId.ProgramId);

        Transaction transaction = new();

        if (mintKey is not null)
        {
            var response = RpcClient.GetAccountInfo(destinationToken, Commitment.Confirmed);
            var lamports = response?.Result?.Value?.Lamports;

            if (lamports is null or 0)
            {
                transaction.Add(AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                    RewardAuthority.RewardAuthority.PublicKey,
                    ticket.UserAddress,
                    mintKey)
                );
            }
        }

        transaction.Add(redeemIx);
        transaction.FeePayer = RewardAuthority.RewardAuthority;
        transaction.RecentBlockHash = RpcClient.GetLatestBlockHash().Result.Value.Blockhash;
        transaction.PartialSign(RewardAuthority.RewardAuthority);
        return transaction;
    }

    private static (PayoutReward, PublicKey?) CreatePayoutReward(TicketRedeemResult redeemResult)
    {
        return redeemResult.RedeemResult switch
        {
            "NONE" => (
                new()
                {
                    Type = PayoutRewardType.NonePayout
                },
                null // new(Enumerable.Repeat(element: 0, count: 32).Select(el => (byte) el).ToArray())
            ),
            "SOL" => (
                new()
                {
                    Type = PayoutRewardType.SolPayout,
                    SolPayoutValue = new()
                    {
                        Amount = (ulong) (redeemResult as TicketSolRedeemResult)!.Amount
                    }
                },
                null //new(Enumerable.Repeat(element: 0, count: 32).Select(el => (byte) el).ToArray())
            ),
            "TOKEN" => (
                new()
                {
                    Type = PayoutRewardType.TokenPayout,
                    TokenPayoutValue = new()
                    {
                        Amount = (ulong) (redeemResult as TicketTokenRedeemResult)!.Amount,
                        Mint = new((redeemResult as TicketTokenRedeemResult)!.Mint)
                    }
                },
                new((redeemResult as TicketTokenRedeemResult)!.Mint)
            ),
            "NFT" => (
                new()
                {
                    Type = PayoutRewardType.NftPayout,
                    NftPayoutValue = new()
                    {
                        Mint = new((redeemResult as TicketNftRedeemResult)!.Mint)
                    }
                },
                new((redeemResult as TicketNftRedeemResult)!.Mint)
            ),
            _ => throw new ArgumentException("Unexpected value", nameof(redeemResult))
        };
    }
}