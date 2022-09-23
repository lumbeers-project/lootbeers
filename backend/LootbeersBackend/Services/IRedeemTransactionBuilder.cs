using LootbeersBackend.Models.Core;
using LootbeersBackend.Models.Dto;
using Solnet.Rpc.Models;
using Solnet.Wallet;

namespace LootbeersBackend.Services;

public interface IRedeemTransactionBuilder
{
    Transaction GetRedeemTransaction(TicketRedeemResult redeemResult, Ticket ticket, PublicKey ticketAddress);
}