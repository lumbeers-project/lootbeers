using System.Data;
using LootbeersBackend.Data;
using LootbeersBackend.Models.Dao;
using LootbeersBackend.Models.Dto;
using LootbeersBackend.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace LootbeersBackend.Actions;

public class LootboxActionException : Exception
{
    public LootboxActionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class LootboxAddException : LootboxActionException
{
    public LootboxAddException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class LootboxAddressAlreadyExists : LootboxAddException
{
    public LootboxAddressAlreadyExists(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class LootboxActions
{
    public LootboxActions(DataContext context, LootboxMapper lootboxMapper)
    {
        Context = context;
        LootboxMapper = lootboxMapper;
    }

    public DataContext Context { get; }

    public LootboxMapper LootboxMapper { get; }

    public async Task AddLootbox(LootboxDto lootbox)
    {
        await using IDbContextTransaction dbTransaction = await Context.Database.BeginTransactionAsync(IsolationLevel.Snapshot);
        Lootbox lootboxDao = LootboxMapper.MapLootboxFromDto(lootbox);
        await Context.Lootboxes.AddAsync(lootboxDao);

        try
        {
            await Context.SaveChangesAsync();
            await dbTransaction.CommitAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException is PostgresException {SqlState: PostgresErrorCodes.UniqueViolation})
            {
                throw new LootboxAddressAlreadyExists("An error occured, see inner exceptions for details", e);
            }
            
            throw new LootboxAddException("An error occured, see inner exceptions for details", e);
        }
    }

    public async Task AddReward(RewardDto reward)
    {
        throw new NotImplementedException();
    }
}