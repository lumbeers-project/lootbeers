using LootbeersBackend.Models.Core.Config;
using LootboxOpener;
using Solnet.Rpc;
using Solnet.Wallet;

namespace LootbeersBackend.Extensions;

public static class StartupExtensions
{
    public static void AddStreamingSolnetClient(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["LootboxOpenerClient:StreamingRpcUrl"];
        services.AddScoped<IStreamingRpcClient>(_ => ClientFactory.GetStreamingClient(url));
    }

    public static void AddSolnetClient(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["LootboxOpenerClient:RpcUrl"];
        services.AddScoped<IRpcClient>(_ => ClientFactory.GetClient(url));
    }

    public static void AddProgramId(this IServiceCollection services, IConfiguration configuration)
    {
        var programId = configuration["LootboxOpenerClient:ProgramId"];

        if (!PublicKey.IsValid(programId))
        {
            throw new ArgumentException("Bad public key value");
        }

        services.AddSingleton<LootboxProgramIdWrapper>(_ => new()
        {
            ProgramId = new(programId)
        });
    }

    public static void AddRootConfigAddress(this IServiceCollection services, IConfiguration configuration)
    {
        var programId = configuration["LootboxOpenerClient:RootConfigAddress"];

        if (!PublicKey.IsValid(programId))
        {
            throw new ArgumentException("Bad public key value");
        }

        services.AddSingleton<RootConfigIdWrapper>(_ => new()
        {
            RootConfigAddress = new(programId)
        });
    }

    public static void AddRewardAuthority(this IServiceCollection services, IConfiguration configuration)
    {
        var seeds = configuration["LootboxOpenerClient:RewardAuthority"];
        Account rewardAuthority = new Wallet(seeds, wordList: null, string.Empty, SeedMode.Bip39).Account;

        services.AddSingleton<RewardAuthorityWrapper>(_ => new()
        {
            RewardAuthority = rewardAuthority
        });
    }

    public static void AddAdminToken(this IServiceCollection services, IConfiguration configuration)
    {
        var adminToken = configuration["Administration:Token"];

        services.AddSingleton<AdminTokenWrapper>(_ => new()
        {
            AccessToken = adminToken
        });
    }

    public static void AddLootboxClient(this IServiceCollection services)
    {
        services.AddScoped<LootboxOpenerClient>(factory =>
        {
            IRpcClient client = factory.GetRequiredService<IRpcClient>();
            IStreamingRpcClient streamingClient = factory.GetRequiredService<IStreamingRpcClient>();
            LootboxProgramIdWrapper programId = factory.GetRequiredService<LootboxProgramIdWrapper>();

            return new(client, streamingClient, programId.ProgramId);
        });
    }

    public static void AddRandom(this IServiceCollection services)
    {
        services.AddTransient(_ => Random.Shared);
    }
}