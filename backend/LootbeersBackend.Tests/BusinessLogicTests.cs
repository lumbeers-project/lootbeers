using LootbeersBackend.Models.Core;
using LootbeersBackend.Models.Dao;
using LootbeersBackend.Models.Dto;
using LootbeersBackend.Services;
using LootbeersBackend.Services.Implementations;
using Moq;
using Solnet.Programs;
using Solnet.Rpc;
using Solnet.Rpc.Models;
using Solnet.Wallet;
using Solnet.Wallet.Bip39;
using RewardType = LootbeersBackend.Models.Dao.RewardType;

namespace LootbeersBackend.Tests;

[TestFixture]
public class BusinessLogicTests
{
    private readonly Lootbox _lootbox = new()
    {
        Id = 1,
        SolanaLootboxAddress = "someAddress",
        Rewards = new List<Reward>
        {
            new NoneReward
            {
                Id = 1,
                Weight = 1,
                RewardType = RewardType.None
            },
            new SolReward
            {
                Id = 2,
                Weight = 1,
                RewardType = RewardType.Sol,
                Amount = 100000UL
            },
            new NftReward
            {
                Id = 3,
                Weight = 1,
                RewardType = RewardType.Nft
                // MintAddress = "nftMintAddress"
            },
            new TokenReward
            {
                Id = 4,
                Weight = 1,
                RewardType = RewardType.Token,
                Amount = 1000UL,
                MintAddress = "tokenMintAddress"
            }
        }
    };

    [Test]
    public void TestResultGenerator()
    {
        var randomShouldReturn = 0;
        var random = new Mock<Random>();

        random.Setup(r =>
                r.Next(It.IsAny<int>())
            )
            // ReSharper disable once AccessToModifiedClosure
            // It is meant to be modified so we can manipulate Random results
            .Returns((int _) => randomShouldReturn);

        IResultGenerator resultGenerator = new ResultGenerator(random.Object);

        randomShouldReturn = 0;

        Assert.Multiple(() =>
        {
            TicketRedeemResult result = resultGenerator.GetResult(_lootbox);
            TicketNoneRedeemResult? typedResult = result as TicketNoneRedeemResult;

            Assert.That(typedResult, Is.Not.Null);
            Assert.That(typedResult!.RedeemResult, Is.EqualTo("NONE"));
        });

        randomShouldReturn = 1;

        Assert.Multiple(() =>
        {
            TicketRedeemResult result = resultGenerator.GetResult(_lootbox);
            TicketSolRedeemResult? typedResult = result as TicketSolRedeemResult;

            Assert.That(typedResult, Is.Not.Null);
            Assert.That(typedResult!.RedeemResult, Is.EqualTo("SOL"));
            Assert.That(typedResult.Amount, Is.EqualTo(100000UL));
        });

        randomShouldReturn = 2;

        Assert.Multiple(() =>
        {
            TicketRedeemResult result = resultGenerator.GetResult(_lootbox);
            TicketNftRedeemResult? typedResult = result as TicketNftRedeemResult;

            Assert.That(typedResult, Is.Not.Null);
            Assert.That(typedResult!.RedeemResult, Is.EqualTo("NFT"));
            Assert.That(typedResult.Mint, Is.EqualTo("ADDRESS_TO_BE_ACCESSED"));
        });

        randomShouldReturn = 3;

        Assert.Multiple(() =>
        {
            TicketRedeemResult result = resultGenerator.GetResult(_lootbox);
            TicketTokenRedeemResult? typedResult = result as TicketTokenRedeemResult;

            Assert.That(typedResult, Is.Not.Null);
            Assert.That(typedResult!.RedeemResult, Is.EqualTo("TOKEN"));
            Assert.That(typedResult.Amount, Is.EqualTo(1000UL));
            Assert.That(typedResult.Mint, Is.EqualTo("tokenMintAddress"));
        });
    }

    [Test]
    public void TestRedeemTransactionBuilder()
    {
        RedeemTransactionBuilder builder = new(
            ClientFactory.GetClient(Cluster.DevNet),
            new()
            {
                ProgramId = new Wallet(WordCount.Twelve, WordList.English, seedMode: SeedMode.Bip39).Account.PublicKey
            },
            new()
            {
                RewardAuthority = new Wallet(WordCount.Twelve, WordList.English, seedMode: SeedMode.Bip39).Account
            },
            new()
            {
                RootConfigAddress = new Wallet(WordCount.Twelve, WordList.English, seedMode: SeedMode.Bip39).Account.PublicKey
            }
        );

        TicketRedeemResult result = new TicketTokenRedeemResult
        {
            Mint = new("mongopjRpUgnQQpQFiasgFLyo69YXUwFcw3hyqaN8RL"),
            Amount = 10_000000000L
        };

        Ticket ticket = new(
            new Wallet(WordCount.Twelve, WordList.English, seedMode: SeedMode.Bip39).Account.PublicKey,
            new Wallet(WordCount.Twelve, WordList.English, seedMode: SeedMode.Bip39).Account.PublicKey
        );

        PublicKey ticketAddress = new Wallet(WordCount.Twelve, WordList.English, seedMode: SeedMode.Bip39).Account.PublicKey;

        Assert.Multiple(() =>
        {
            Transaction transaction = null!;

            Assert.DoesNotThrow(() =>
            {
                transaction = builder.GetRedeemTransaction(result, ticket, ticketAddress);
            });

            Assert.That(transaction, Is.Not.Null);
            Assert.That(transaction.Instructions, Has.Count.EqualTo(2));
        });
    }

    [Test]
    public void TestTransactionSender()
    {
        IRpcClient client = ClientFactory.GetClient(Cluster.DevNet);

        IBlockchainInvoke invoke = new BlockchainInvoke(client);

        Account source = new Wallet(
                "romance trip put vacant bring chaos mixture crystal joy soul crop bench",
                wordList: null,
                string.Empty,
                SeedMode.Bip39
            )
            .Account;

        Account destination = new Wallet(WordCount.Twelve, WordList.English, seedMode: SeedMode.Bip39).Account;

        Transaction transaction = new()
        {
            FeePayer = source,
            RecentBlockHash = client.GetLatestBlockHash().Result.Value.Blockhash
        };

        transaction.Add(SystemProgram.Transfer(source, destination, lamports: 0_001_000_000UL)); // Transfer 0.001 SOL
        transaction.Add(SystemProgram.Transfer(destination, source, lamports: 0_001_000_000UL)); // Transfer 0.001 SOL back
        transaction.Sign(new[] {source, destination});

        Assert.Multiple(() =>
        {
            var result = string.Empty;

            Assert.DoesNotThrow(() =>
            {
                result = invoke.SendTransaction(transaction).GetAwaiter().GetResult();
            });

            Assert.That(result, Is.Not.Empty);
        });

        transaction = new()
        {
            FeePayer = source,
            RecentBlockHash = client.GetLatestBlockHash().Result.Value.Blockhash
        };

        transaction.Add(SystemProgram.Transfer(source, destination, lamports: 0_001_000_000UL)); // Transfer 0.001 SOL
        transaction.Add(SystemProgram.Transfer(destination, source, lamports: 0_001_000_000UL)); // Transfer 0.001 SOL back
        transaction.Sign(source); // One signer is missing

        Assert.Throws<BlockchainInvokeException>(() =>
        {
            invoke.SendTransaction(transaction).GetAwaiter().GetResult();
        });
    }
}