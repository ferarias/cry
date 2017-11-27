using System.Collections.Generic;
using System.Text;

namespace CryCompareApi
{
    public class RawDisplayResponse
    {
        public Dictionary<string, Dictionary<string, CoinFull>> Raw {get; set;}

        public Dictionary<string, Dictionary<string, CoinFull>> Display {get; set;}
    }
}