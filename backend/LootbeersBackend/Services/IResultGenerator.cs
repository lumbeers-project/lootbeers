using LootbeersBackend.Models.Dao;
using LootbeersBackend.Models.Dto;

namespace LootbeersBackend.Services;

public interface IResultGenerator
{
    TicketRedeemResult GetResult(Lootbox lootbox);
}