namespace CryCompareApi
{
    public class CoinFull
    {
        int Type { get; set; }
        string Market { get; set; }
        string FromSymbol { get; set; }
        string ToSymbol { get; set; }
        string Flags { get; set; }
        double Price { get; set; }
        double LastUpdate { get; set; }
        double LastVolume { get; set; }
        double LastVolumeTo { get; set; }
        double LastTradeId { get; set; }
        double VolumeDay { get; set; }
        double VolumeDayTo { get; set; }
        double Volume24hour { get; set; }
        double Volume24hourTo { get; set; }
        double OpenDay { get; set; }
        double HighDay { get; set; }
        double LowDay { get; set; }
        double Open24hour { get; set; }
        double High24hour { get; set; }
        double Low24hour { get; set; }
        string LastMarket { get; set; }
        double Change24hour { get; set; }
        double ChangePct24hour { get; set; }
        double ChangeDay { get; set; }
        double ChangePctDay { get; set; }
        double Supply { get; set; }
        double MktCap { get; set; }
        double TotalVolume24h { get; set; }
        double TotalVolume24hto { get; set; }
    }
}