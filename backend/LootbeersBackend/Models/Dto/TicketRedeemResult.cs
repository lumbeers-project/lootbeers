namespace LootbeersBackend.Models.Dto;

public abstract class TicketRedeemResult
{
    public abstract string RedeemResult { get; }
    
    public int Id { get; set; }
}

public class TicketSolRedeemResult : TicketRedeemResult
{
    public override string RedeemResult => "SOL";

    public long Amount { get; init; }
}

public class TicketTokenRedeemResult : TicketRedeemResult
{
    public override string RedeemResult => "TOKEN";

    public string Mint { get; init; } = null!;

    public long Amount { get; init; }
}

public class TicketNftRedeemResult : TicketRedeemResult
{
    public override string RedeemResult => "NFT";

    public string Mint { get; init; } = null!;
}

public class TicketNoneRedeemResult : TicketRedeemResult
{
    public override string RedeemResult => "NONE";
}