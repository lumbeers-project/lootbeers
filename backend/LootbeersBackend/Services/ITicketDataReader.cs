using LootbeersBackend.Models.Core;
using Solnet.Wallet;

namespace LootbeersBackend.Services;

public interface ITicketDataReader
{
    public Task<Ticket> ReadTicket(PublicKey ticketAddress);
}