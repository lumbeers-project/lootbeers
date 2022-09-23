using System.Data;
using System.Runtime.Serialization;
using LootbeersBackend.Data;
using LootbeersBackend.Models.Core;
using LootbeersBackend.Models.Dao;
using LootbeersBackend.Models.Dto;
using LootbeersBackend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Solnet.Rpc.Models;
using Solnet.Wallet;

namespace LootbeersBackend.Actions;

public class RedeemException : Exception
{
    protected RedeemException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public RedeemException(string? message) : base(message)
    {
    }

    public RedeemException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class LootboxNotFoundException : RedeemException
{
    protected LootboxNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public LootboxNotFoundException(string? message) : base(message)
    {
    }

    public LootboxNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class TicketActions
{
    public TicketActions(ITicketDataReader ticketDataReader, IResultGenerator resultGenerator, IBlockchainInvoke blockchainInvoke, IRedeemTransactionBuilder redeemTransactionBuilder, DataContext context)
    {
        TicketDataReader = ticketDataReader;
        ResultGenerator = resultGenerator;
        BlockchainInvoke = blockchainInvoke;
        RedeemTransactionBuilder = redeemTransactionBuilder;
        Context = context;
    }

    public ITicketDataReader TicketDataReader { get; set; }

    public IResultGenerator ResultGenerator { get; set; }
    
    public IBlockchainInvoke BlockchainInvoke { get; set; }
    
    public IRedeemTransactionBuilder RedeemTransactionBuilder { get; set; }
    
    public DataContext Context { get; set; }

    public async Task<TicketRedeemResult> RedeemTicket(PublicKey ticketAddress)
    {
        // Order:
        // Get ticket data (OK)
        // Find lootbox (OK)
        // Get result (OK)
        // Begin database transaction (OK)
        // Write result to database (TO DO)
        // Send solana transaction (OK)
        // Commit database transaction (OK)
        // Return generated result (OK)

        Ticket ticket = await TicketDataReader.ReadTicket(ticketAddress);
        PublicKey lootboxAddress = ticket.LootboxAddress;

        await using IDbContextTransaction dbTransaction = await Context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        Lootbox lootbox = await Context.Lootboxes
                              .AsNoTracking()
                              .Include(l => l.Rewards)
                              .FirstOrDefaultAsync(l => l.SolanaLootboxAddress == lootboxAddress) ??
                          throw new LootboxNotFoundException("Lootbox was not found in database");

        TicketRedeemResult lootboxOpenResult = ResultGenerator.GetResult(lootbox);

        // Here we should update NFT info

        // Here we should send transaction
        Transaction transaction = RedeemTransactionBuilder.GetRedeemTransaction(lootboxOpenResult, ticket, ticketAddress);

        try
        {
            await BlockchainInvoke.SendTransaction(transaction);
        }
        catch (BlockchainInvokeException e)
        {
            throw new RedeemException("An error occured, check inner exception for details", e);
        }

        await dbTransaction.CommitAsync();
        return lootboxOpenResult;
    }
}