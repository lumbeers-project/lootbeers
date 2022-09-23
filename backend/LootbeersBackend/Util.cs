namespace LootbeersBackend;

public static class Util
{
    public static IHostBuilder CreateDefaultBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(options =>
                options.UseStartup<Startup>());
    }

    [Obsolete("Use moq to create dependency injection context")]
    public static IHostBuilder CreateBuilderWithStartupOverride<TStartup>(string[] args) where TStartup : class
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(options =>
                options.UseStartup<TStartup>());
    }
}