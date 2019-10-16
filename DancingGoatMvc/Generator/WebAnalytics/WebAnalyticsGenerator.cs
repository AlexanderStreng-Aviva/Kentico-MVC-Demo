using CMS.SiteProvider;

namespace DancingGoat.Generator.WebAnalytics
{
    public class WebAnalyticsGenerator
    {
        public void Generate(int siteId)
        {
            GenerateOnlineMarketingData(SiteInfoProvider.GetSiteInfo(siteId));
        }

        private static void GenerateOnlineMarketingData(SiteInfo site)
        {
            new CampaignContactsDataGenerator("Campaign.Info").Generate();
            new CampaignDataGenerator(site, "Campaign.Info").Generate();
            new AbTestAndConversionDataGenerator(site).Generate();
            new OnlineMarketingDataGenerator(site).Generate();
            new ContactGroupSubscribersDataGenerator(site).Generate();
            new ScoringWithRulesGenerator(site).Generate();
            new NewslettersDataGenerator(site).Generate();
            new PersonaWithRulesGenerator(site).Generate();
            new WebAnalyticsDataGenerator(site).Generate();
        }
    }
}