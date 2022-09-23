using LootbeersBackend;

IHost app = Util.CreateDefaultBuilder(args).Build();
await app.RunAsync();