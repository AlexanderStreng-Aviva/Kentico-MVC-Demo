using CMS.SiteProvider;

namespace DancingGoat.Generator.WebAnalytics
{
    public class WebAnalyticsGenerator
    {
        private const string ContactFirstNamePrefix = "GeneratedCampaignContact";
        private const string ContactLastNamePrefix = "GeneratedCampaignContactLastName";

        public void Generate(int siteId)
        {
            GenerateOnlineMarketingData(SiteInfoProvider.GetSiteInfo(siteId));
        }

        private static void GenerateOnlineMarketingData(SiteInfo site)
        {
            new CampaignContactsDataGenerator(ContactFirstNamePrefix, ContactLastNamePrefix).Generate();
            new CampaignDataGenerator(site, "GeneratedCampaignContact").Generate();
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