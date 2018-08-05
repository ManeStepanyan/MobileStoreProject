namespace MobileStore.Src.Models
{
    public class SearchProductModel
    {
        public string Brand { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int? MinRAM { get; set; }
        public int? MaxRAM { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public int? MinBattery { get; set; }
        public int? MaxBattery { get; set; }
        public int? MinCamera { get; set; }
        public int? MaxCamera { get; set; }
        public int? MinMemory { get; set; }
        public int? MaxMemory { get; set; }
    }
}