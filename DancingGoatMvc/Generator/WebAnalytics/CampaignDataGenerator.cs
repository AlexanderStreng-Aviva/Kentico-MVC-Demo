using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.OnlineForms;
using CMS.Scheduler;
using CMS.SiteProvider;
using CMS.WebAnalytics;

namespace DancingGoat.Generator.WebAnalytics
{
    public class CampaignDataGenerator
    {
        private const string CampaignCoffeeClubMembershipDraft = "DancingGoat.CoffeeClubMembership";
        private const string CampaignCoffeeClubMembershipScheduled = "DancingGoat.CoffeeClubMembershipTest";
        private const string CampaignCafeSamplePromotionRunning = "DancingGoat.CafeSamplePromotion";
        private const string CampaignCafeSamplePromotionFinished = "DancingGoat.CafeSamplePromotionTest";
        private const string ConversionPagevisitColombia = "Colombia";
        private const string ConversionPagevisitAmericasCoffeePoster = "America's coffee poster";
        private const string ConversionFormsubmissionTryFreeSample = "Try a free sample";
        private const string ConversionUserregistration = "User registration";
        private const string PagePathColombia = "/Campaign-assets/Cafe-promotion/Colombia";
        private const string PagePathThankYou = "/Campaign-assets/Cafe-promotion/Thank-you";

        private const string PagePathAmericasCoffeePoster =
            "/Campaign-assets/Cafe-promotion/America-s-coffee-poster";

        private const string PagePathCoffeeClubMembership = "/Store/Coffee-Club-Membership";
        private const string TryFreeSampleFormCodeName = "TryAFreeSample";
        private const int CampaignCafeSamplePromotionFinishedContactsCount = 100;
        private const int CampaignCafeSamplePromotionRunningContactsCount = 0;

        private readonly Dictionary<string, IEnumerable<ActivityDataParameters>>
            _campaignCafeSamplePromotionFinishedHits = new Dictionary<string, IEnumerable<ActivityDataParameters>>
            {
                {
                    "Colombia",
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            string.Empty, 7),
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            "colombia_mail_1", 13),
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            "colombia_mail_2", 16),
                        GetActivityDataParameters("facebook", "fb_colombia", 30),
                        GetActivityDataParameters("twitter", string.Empty, 4),
                        GetActivityDataParameters("twitter", "twitter_post_1", 42),
                        GetActivityDataParameters("twitter", "twitter_post_2", 21)
                    }
                },
                {
                    "Try a free sample",
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            string.Empty, 5),
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            "colombia_mail_1", 11),
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            "colombia_mail_2", 15),
                        GetActivityDataParameters("facebook", string.Empty, 4),
                        GetActivityDataParameters("facebook", "fb_colombia", 21),
                        GetActivityDataParameters("twitter", "twitter_post_1", 9),
                        GetActivityDataParameters("twitter", "twitter_post_2", 7)
                    }
                },
                {
                    "America's coffee poster",
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            "colombia_mail_1", 5),
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            "colombia_mail_2", 3),
                        GetActivityDataParameters("facebook", "fb_colombia", 11),
                        GetActivityDataParameters("twitter", "twitter_post_1", 5),
                        GetActivityDataParameters("twitter", "twitter_post_2", 8),
                        GetActivityDataParameters("linkedin", string.Empty, 6)
                    }
                },
                {
                    "User registration",
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            string.Empty, 4),
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            "colombia_mail_1", 29),
                        GetActivityDataParameters("colombia_coffee_sample_promotion",
                            "colombia_mail_2", 17)
                    }
                }
            };

        private readonly Dictionary<string, IEnumerable<ActivityDataParameters>>
            _campaignCafeSamplePromotionRunningHits = new Dictionary<string, IEnumerable<ActivityDataParameters>>
            {
                {
                    "Colombia",
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("linkedin", "linkedin_colombia", 1429),
                        GetActivityDataParameters("facebook", string.Empty, 66),
                        GetActivityDataParameters("facebook", "fb_colombia_1", 1246),
                        GetActivityDataParameters("facebook", "fb_colombia_2", 1152),
                        GetActivityDataParameters("twitter", "twitter_colombia", 310)
                    }
                },
                {
                    "Try a free sample",
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("linkedin", "linkedin_colombia", 175),
                        GetActivityDataParameters("facebook", string.Empty, 77),
                        GetActivityDataParameters("facebook", "fb_colombia_1", 248),
                        GetActivityDataParameters("facebook", "fb_colombia_2", 173),
                        GetActivityDataParameters("twitter", "twitter_colombia", 58)
                    }
                },
                {
                    "America's coffee poster",
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("linkedin", string.Empty, 42),
                        GetActivityDataParameters("linkedin", "linkedin_colombia", 45),
                        GetActivityDataParameters("facebook", "fb_colombia_1", 110),
                        GetActivityDataParameters("facebook", "fb_colombia_2", 96),
                        GetActivityDataParameters("twitter", "twitter_colombia", 10)
                    }
                },
                {
                    "User registration",
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("linkedin", "linkedin_colombia", 50),
                        GetActivityDataParameters("facebook", "fb_colombia_1", 43),
                        GetActivityDataParameters("facebook", "fb_colombia_2", 42),
                        GetActivityDataParameters("twitter", string.Empty, 3),
                        GetActivityDataParameters("twitter", "twitter_colombia", 12)
                    }
                }
            };

        private readonly string _mContactFirstNamePrefix;

        private readonly SiteInfo _mSite;

        private readonly Guid _newsletterCoffeeClubMembershipIssueGuid =
            Guid.Parse("5045B325-F360-4536-8692-9454FA91EBAA");

        private readonly Guid _newsletterColombiaCoffeeSamplePromotionIssueGuid =
            Guid.Parse("C818B404-0488-4558-B438-08167DE75824");

        public CampaignDataGenerator(SiteInfo site, string contactFirstNamePrefix)
        {
            _mSite = site;
            _mContactFirstNamePrefix = contactFirstNamePrefix;
        }

        private static ActivityDataParameters GetActivityDataParameters(
            string utmSource,
            string utmContent,
            int count)
        {
            return new ActivityDataParameters
            {
                UtmSource = utmSource,
                UtmContent = utmContent,
                Count = count
            };
        }

        public void Generate()
        {
            CampaignInfoProvider.GetCampaigns().WhereStartsWith("CampaignName", "DancingGoat.")
                .OnSite(_mSite.SiteID).ToList()
                .ForEach(CampaignInfoProvider.DeleteCampaignInfo);
            GenerateCoffeeClubMembershipCampaign();
            GenerateCafePromotionSampleCampaign();
            GenerateCampaignObjective("DancingGoat.CafeSamplePromotion", "Try a free sample", 600);
            GenerateCampaignObjective("DancingGoat.CafeSamplePromotionTest", "Try a free sample", 50);
            GenerateActivities("DancingGoat.CafeSamplePromotion", _campaignCafeSamplePromotionRunningHits,
                0);
            GenerateActivities("DancingGoat.CafeSamplePromotionTest",
                _campaignCafeSamplePromotionFinishedHits, 100);
            new CalculateCampaignConversionReportTask().Execute(new TaskInfo
            {
                TaskSiteID = _mSite.SiteID
            });
        }

        private void GenerateCampaignObjective(
            string campaignName,
            string conversionName,
            int objectiveValue)
        {
            var campaignInfo = CampaignInfoProvider.GetCampaignInfo(campaignName, _mSite.SiteName);
            if (campaignInfo == null)
                return;
            var campaignConversionInfo = CampaignConversionInfoProvider.GetCampaignConversions()
                .WhereEquals("CampaignConversionDisplayName", conversionName)
                .WhereEquals("CampaignConversionCampaignID", campaignInfo.CampaignID)
                .FirstOrDefault();
            if (campaignConversionInfo == null)
                return;
            CampaignObjectiveInfoProvider.SetCampaignObjectiveInfo(new CampaignObjectiveInfo
            {
                CampaignObjectiveCampaignID = campaignInfo.CampaignID,
                CampaignObjectiveCampaignConversionID = campaignConversionInfo.CampaignConversionID,
                CampaignObjectiveValue = objectiveValue
            });
        }

        private void GenerateCampaign(CampaignData campaignData)
        {
            var siteName = _mSite.SiteName;
            if (CampaignInfoProvider.GetCampaignInfo(campaignData.CampaignName, siteName) != null)
                return;
            var campaignInfo = new CampaignInfo
            {
                CampaignName = campaignData.CampaignName,
                CampaignDisplayName = campaignData.CampaignDisplayName,
                CampaignDescription = campaignData.CampaignDescription,
                CampaignOpenFrom = campaignData.CampaignOpenFrom,
                CampaignCalculatedTo = campaignData.CampaignOpenFrom,
                CampaignOpenTo = campaignData.CampaignOpenTo,
                CampaignSiteID = _mSite.SiteID,
                CampaignUTMCode = campaignData.CampaignUtmCode
            };
            CampaignInfoProvider.SetCampaignInfo(campaignInfo);
            CampaignDataGeneratorHelpers.AddNewsletterAsset(campaignInfo, campaignData.CampaignEmailPromotion);
            var infoByGuid = ProviderHelper.GetInfoByGuid("newsletter.issue", campaignData.CampaignEmailPromotion,
                campaignInfo.CampaignSiteID);
            if (infoByGuid != null)
            {
                infoByGuid.SetValue("IssueStatus", 5);
                infoByGuid.SetValue("IssueMailoutTime", campaignData.CampaignOpenFrom);
                infoByGuid.Update();
            }

            foreach (var pagePath in campaignData.CampaignContentInventory)
                CampaignDataGeneratorHelpers.AddPageAsset(campaignInfo.CampaignID, pagePath);
            foreach (var conversionData in campaignData.CampaignReportSetup)
                CampaignDataGeneratorHelpers.CreateConversion(campaignInfo.CampaignID, conversionData);
        }

        private void GenerateCoffeeClubMembershipCampaign()
        {
            var campaignData = new CampaignData
            {
                CampaignName = "DancingGoat.CoffeeClubMembership",
                CampaignDisplayName = "Coffee club membership",
                CampaignDescription =
                    "The goal of this campaign is to promote the Coffee Club, a new service that the Dancing Goat company provides for it's coffee geek customers.",
                CampaignUtmCode = "coffee_club_membership_draft",
                CampaignOpenFrom = DateTime.MinValue,
                CampaignOpenTo = DateTime.MinValue,
                CampaignEmailPromotion = _newsletterCoffeeClubMembershipIssueGuid,
                CampaignContentInventory = new List<string>
                {
                    "/Products/Coffees/Ethiopia-Yirgacheffe"
                },
                CampaignReportSetup = PrepareCoffeeClubMembershipConversions()
            };
            GenerateCampaign(campaignData);
            campaignData.CampaignName = "DancingGoat.CoffeeClubMembershipTest";
            campaignData.CampaignDisplayName = "Coffee club membership test";
            campaignData.CampaignOpenFrom = DateTime.Now.AddDays(6.0);
            campaignData.CampaignUtmCode = "coffee_club_membership_scheduled";
            GenerateCampaign(campaignData);
        }

        private void GenerateCafePromotionSampleCampaign()
        {
            var campaignData = new CampaignData
            {
                CampaignName = "DancingGoat.CafeSamplePromotion",
                CampaignDisplayName = "Cafe sample promotion",
                CampaignDescription =
                    "The goal of this campaign is to increase the number of visitors in our cafes. We want to achieve that by sending out free coffee sample coupons that customers can redeem at the cafes. At the end of the process a poster download is offered to see whether people would be interested in such freebies.",
                CampaignUtmCode = "cafe_sample_promotion_running",
                CampaignOpenFrom = DateTime.Now.AddDays(-14.0),
                CampaignOpenTo = DateTime.MinValue,
                CampaignEmailPromotion = _newsletterColombiaCoffeeSamplePromotionIssueGuid,
                CampaignContentInventory = new List<string>
                {
                    "/Campaign-assets/Cafe-promotion/Colombia",
                    "/Campaign-assets/Cafe-promotion/Thank-you",
                    "/Campaign-assets/Cafe-promotion/America-s-coffee-poster"
                },
                CampaignReportSetup = PrepareCafeSamplePromotionConversions()
            };
            GenerateCampaign(campaignData);
            campaignData.CampaignName = "DancingGoat.CafeSamplePromotionTest";
            campaignData.CampaignDisplayName = "Cafe sample promotion test";
            campaignData.CampaignOpenTo = campaignData.CampaignOpenFrom.AddDays(6.0);
            campaignData.CampaignUtmCode = "cafe_sample_promotion_finished";
            GenerateCampaign(campaignData);
        }

        private void GenerateActivities(
            string campaignName,
            Dictionary<string, IEnumerable<ActivityDataParameters>> conversionHits,
            int contactsCount)
        {
            var siteName = _mSite.SiteName;
            var campaignInfo = CampaignInfoProvider.GetCampaignInfo(campaignName, siteName);
            var document1 =
                CampaignDataGeneratorHelpers.GetDocument("/Campaign-assets/Cafe-promotion/America-s-coffee-poster");
            var document2 = CampaignDataGeneratorHelpers.GetDocument("/Campaign-assets/Cafe-promotion/Colombia");
            var bizFormInfo = BizFormInfoProvider.GetBizFormInfo("TryAFreeSample", _mSite.SiteID);
            CampaignDataGeneratorHelpers.DeleteOldActivities(campaignInfo.CampaignUTMCode);
            var contactsIDs = new ContactsIdData(_mContactFirstNamePrefix, contactsCount);
            CampaignDataGeneratorHelpers.GenerateActivities(conversionHits["Colombia"], campaignInfo, "pagevisit",
                contactsIDs, document2.NodeID);
            CampaignDataGeneratorHelpers.GenerateActivities(conversionHits["America's coffee poster"], campaignInfo,
                "pagevisit", contactsIDs, document1.NodeID);
            CampaignDataGeneratorHelpers.GenerateActivities(conversionHits["User registration"], campaignInfo,
                "userregistration", contactsIDs);
            CampaignDataGeneratorHelpers.GenerateActivities(conversionHits["Try a free sample"], campaignInfo,
                "bizformsubmit", contactsIDs, bizFormInfo.FormID);
        }

        private IEnumerable<CampaignConversionData> PrepareCafeSamplePromotionConversions()
        {
            var document1 =
                CampaignDataGeneratorHelpers.GetDocument("/Campaign-assets/Cafe-promotion/America-s-coffee-poster");
            var document2 = CampaignDataGeneratorHelpers.GetDocument("/Campaign-assets/Cafe-promotion/Colombia");
            var bizFormInfo = BizFormInfoProvider.GetBizFormInfo("TryAFreeSample", _mSite.SiteID);
            return new List<CampaignConversionData>
            {
                new CampaignConversionData
                {
                    ConversionName = "try_free_sample",
                    ConversionDisplayName = "Try a free sample",
                    ConversionActivityType = "bizformsubmit",
                    ConversionItemId = bizFormInfo.FormID,
                    ConversionOrder = 1,
                    ConversionIsFunnelStep = false
                },
                new CampaignConversionData
                {
                    ConversionName = "america_coffee_poster",
                    ConversionDisplayName = document1.DocumentName,
                    ConversionActivityType = "pagevisit",
                    ConversionItemId = document1.NodeID,
                    ConversionOrder = 2,
                    ConversionIsFunnelStep = false
                },
                new CampaignConversionData
                {
                    ConversionName = "userregistration",
                    ConversionDisplayName = "",
                    ConversionActivityType = "userregistration",
                    ConversionItemId = new int?(),
                    ConversionOrder = 3,
                    ConversionIsFunnelStep = false
                },
                new CampaignConversionData
                {
                    ConversionName = "colombia",
                    ConversionDisplayName = document2.DocumentName,
                    ConversionActivityType = "pagevisit",
                    ConversionItemId = document2.NodeID,
                    ConversionOrder = 1,
                    ConversionIsFunnelStep = true
                },
                new CampaignConversionData
                {
                    ConversionName = "try_free_sample_1",
                    ConversionDisplayName = "Try a free sample",
                    ConversionActivityType = "bizformsubmit",
                    ConversionItemId = bizFormInfo.FormID,
                    ConversionOrder = 2,
                    ConversionIsFunnelStep = true
                },
                new CampaignConversionData
                {
                    ConversionName = "america_coffee_poster_1",
                    ConversionDisplayName = document1.DocumentName,
                    ConversionActivityType = "pagevisit",
                    ConversionItemId = document1.NodeID,
                    ConversionOrder = 3,
                    ConversionIsFunnelStep = true
                }
            };
        }

        private IEnumerable<CampaignConversionData> PrepareCoffeeClubMembershipConversions()
        {
            var document = CampaignDataGeneratorHelpers.GetDocument("/Products/Coffees/Ethiopia-Yirgacheffe");
            return new List<CampaignConversionData>
            {
                new CampaignConversionData
                {
                    ConversionName = "coffee_club_membership",
                    ConversionDisplayName = document.DocumentName,
                    ConversionActivityType = "purchasedproduct",
                    ConversionItemId = document.NodeSKUID,
                    ConversionOrder = 1,
                    ConversionIsFunnelStep = false
                },
                new CampaignConversionData
                {
                    ConversionName = "coffee_club_membership_1",
                    ConversionDisplayName = document.DocumentName,
                    ConversionActivityType = "pagevisit",
                    ConversionItemId = document.NodeID,
                    ConversionOrder = 1,
                    ConversionIsFunnelStep = true
                },
                new CampaignConversionData
                {
                    ConversionName = "coffee_club_membership_2",
                    ConversionDisplayName = document.DocumentName,
                    ConversionActivityType = "productaddedtoshoppingcart",
                    ConversionItemId = document.NodeSKUID,
                    ConversionOrder = 2,
                    ConversionIsFunnelStep = true
                },
                new CampaignConversionData
                {
                    ConversionName = "coffee_club_membership_3",
                    ConversionDisplayName = document.DocumentName,
                    ConversionActivityType = "purchasedproduct",
                    ConversionItemId = document.NodeSKUID,
                    ConversionOrder = 3,
                    ConversionIsFunnelStep = true
                }
            };
        }
    }
}