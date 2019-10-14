namespace DancingGoat.Generator.WebAnalytics
{
    public class CampaignConversionData
    {
        public string ConversionName { get; set; }

        public string ConversionDisplayName { get; set; }

        public string ConversionActivityType { get; set; }

        public int? ConversionItemId { get; set; }

        public int ConversionOrder { get; set; }

        public bool ConversionIsFunnelStep { get; set; }
    }
}