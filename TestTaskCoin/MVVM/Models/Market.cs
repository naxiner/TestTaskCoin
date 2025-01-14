namespace TestTaskCoin.MVVM.Models
{
    public class Market
    {
        public string ExchangeId { get; set; }
        public string BaseId { get; set; }
        public string QuoteId { get; set; }
        public string BaseSymbol { get; set; }
        public string QuoteSymbol { get; set; }
        public decimal VolumeUsd24Hr { get; set; }
        public decimal PriceUsd { get; set; }
        public decimal VolumePercent { get; set; }
    }
}
