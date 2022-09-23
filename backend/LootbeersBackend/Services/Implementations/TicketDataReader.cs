using System.Runtime.Serialization;
using LootbeersBackend.Models.Core;
using LootboxOpener;
using Microsoft.AspNetCore.Components;
using Solnet.Rpc.Types;
using Solnet.Wallet;

namespace LootbeersBackend.Services.Implementations;

public class TicketDataReaderException : Exception
{
    public TicketDataReaderException()
    {
    }

    protected TicketDataReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public TicketDataReaderException(string? message) : base(message)
    {
    }

    public TicketDataReaderException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class TicketDataReader : ITicketDataReader
{
    public TicketDataReader(LootboxOpenerClient lootboxOpenerClient)
    {
        LootboxOpenerClient = lootboxOpenerClient;
    }

    public LootboxOpenerClient LootboxOpenerClient { get; set; }

    public async Task<Ticket> ReadTicket(PublicKey ticketAddress)
    {
        try
        {
            var ticketData = await LootboxOpenerClient.GetTicketAsync(ticketAddress.Key, Commitment.Confirmed) ??
                             throw new NullReferenceException("Excepted to get non-null value in response");

            return new(ticketData.ParsedResult.LootboxAddress, ticketData.ParsedResult.User);
        }
        catch (Exception e)
        {
            throw new TicketDataReaderException("An exception occured, see inner exception for details", e);
        }
    }
}