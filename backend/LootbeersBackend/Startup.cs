using System.Text.Json.Serialization;
using LootbeersBackend.Actions;
using LootbeersBackend.Data;
using LootbeersBackend.Extensions;
using LootbeersBackend.Services;
using LootbeersBackend.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace LootbeersBackend;

internal class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DataContext");
        var password = Configuration["PostgresPassword"];
        connectionString = string.Format(connectionString, password);

        services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

        services.AddCors(options => options.AddDefaultPolicy(policy =>
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
        ));

        services.AddAuthorization();

        services.AddCors(builder =>
            builder.AddDefaultPolicy(
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            )
        );

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        services.AddScoped<TicketActions>();
        services.AddScoped<LootboxActions>();
        services.AddScoped<LootboxMapper>();
        services.AddRandom();
        services.AddScoped<ITicketDataReader, TicketDataReader>();
        services.AddScoped<IResultGenerator, ResultGenerator>();
        services.AddScoped<IBlockchainInvoke, BlockchainInvoke>();
        services.AddScoped<IRedeemTransactionBuilder, RedeemTransactionBuilder>();

        services.AddSolnetClient(Configuration);
        services.AddStreamingSolnetClient(Configuration);
        services.AddProgramId(Configuration);
        services.AddRootConfigAddress(Configuration);
        services.AddRewardAuthority(Configuration);
        services.AddLootboxClient();
        services.AddAdminToken(Configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors();

        app.UseEndpoints(endpoint => endpoint.MapDefaultControllerRoute());
    }
}