using System;

namespace CryCompareApi
{
    public class Coin
    {
        public int Id { get; set; }
        public Uri Url { get; set; }
        public Uri ImageUrl { get; set; }
        public string Name { get; set; }
        public string CoinName { get; set; }
        public string FullName { get; set; }
        public string Algorithm { get; set; }
        public string ProofType { get; set; }
        public int SortOrder { get; set; }
    }
}
