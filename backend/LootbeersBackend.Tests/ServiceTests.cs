using LootbeersBackend.Actions;
using LootbeersBackend.Data;
using LootbeersBackend.Services;
using LootboxOpener;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LootbeersBackend.Tests;

public class ServiceTests
{
    private IHost _host = null!;

    [SetUp]
    public void Setup()
    {
        IHostBuilder hostBuilder = Util.CreateDefaultBuilder(new[]
        {
            "--environment",
            "Development"
        });

        _host = hostBuilder.Build();
    }

    [Test]
    public void TestDatabaseConnection()
    {
        Assert.That(_host, Is.Not.Null);
        using IServiceScope scope = _host.Services.CreateScope();
        DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();

        Assert.That(context.Database.CanConnect(), Is.True);
    }

    [Test]
    public void TestDependencyInjection()
    {
        Assert.That(_host, Is.Not.Null);
        using IServiceScope scope = _host.Services.CreateScope();

        Assert.DoesNotThrow(() => scope.ServiceProvider.GetRequiredService<DataContext>());
        Assert.DoesNotThrow(() => scope.ServiceProvider.GetRequiredService<TicketActions>());
        Assert.DoesNotThrow(() => scope.ServiceProvider.GetRequiredService<LootboxActions>());
        Assert.DoesNotThrow(() => scope.ServiceProvider.GetRequiredService<ITicketDataReader>());
        Assert.DoesNotThrow(() => scope.ServiceProvider.GetRequiredService<IResultGenerator>());
        Assert.DoesNotThrow(() => scope.ServiceProvider.GetRequiredService<IBlockchainInvoke>());
        Assert.DoesNotThrow(() => scope.ServiceProvider.GetRequiredService<IRedeemTransactionBuilder>());
        Assert.DoesNotThrow(() => scope.ServiceProvider.GetRequiredService<LootboxOpenerClient>());
    }
}