namespace MyView.Models
{
    public class VMNightMarket
    {
        public List<NightMarket> NightMarkets { get; set; } = null!; //用於左邊導覽列的多項資料
        public NightMarket NightMarket { get; set; } = null!; //用於右邊顯示的單項資料
    }
}
