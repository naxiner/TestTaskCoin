using TestTaskCoin.Enums;

namespace TestTaskCoin.MVVM.Models
{
    public class HistoryRequest
    {
        public string Id { get; set; }
        public HistoryInterval Interval { get; set; }
        public long? Start { get; set; }
        public long? End { get; set; }
    }
}
