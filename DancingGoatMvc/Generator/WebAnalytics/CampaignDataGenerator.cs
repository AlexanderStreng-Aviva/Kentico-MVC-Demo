using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Core;
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

        private const string PagePathAmericasCoffeePoster = "/Campaign-assets/Cafe-promotion/America-s-coffee-poster";

        private const string PagePathCoffeeClubMembership = "/Campaign-assets/Cafe-promotion/Coffee-Club-Membership";
        private const string TryFreeSampleFormCodeName = "TryAFreeSample";

        private const int CampaignCafeSamplePromotionFinishedContactsCount = 1000;
        private const int CampaignCafeSamplePromotionRunningContactsCount = 500;

        private readonly Dictionary<string, IEnumerable<ActivityDataParameters>>
            _campaignCafeSamplePromotionFinishedHits = new Dictionary<string, IEnumerable<ActivityDataParameters>>
            {
                {
                    ConversionPagevisitColombia,
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("colombia_coffee_sample_promotion",string.Empty, 7),
                        GetActivityDataParameters("colombia_coffee_sample_promotion","colombia_mail_1", 13),
                        GetActivityDataParameters("colombia_coffee_sample_promotion","colombia_mail_2", 16),
                        GetActivityDataParameters("facebook", "fb_colombia", 30),
                        GetActivityDataParameters("twitter", string.Empty, 4),
                        GetActivityDataParameters("twitter", "twitter_post_1", 42),
                        GetActivityDataParameters("twitter", "twitter_post_2", 21)
                    }
                },
                {
                    ConversionFormsubmissionTryFreeSample,
                    new List<ActivityDataParameters>
                    {
                        GetActivityDataParameters("colombia_coffee_sample_promotion",string.Empty, 5),
                        GetActivityDataParameters("colombia_coffee_sample_promotion","colombia_mail_1", 11),
                        GetActivityDataParameters("colombia_coffee_sample_promotion","colombia_mail_2", 15),
                        GetActivityDataParameters("facebook", string.Empty, 4),
                        GetActivityDataParameters("facebook", "fb_colombia", 21),
                        GetActivityDataParameters("twitter", "twitter_post_1", 9),
                        GetActivityDataParameters("twitter", "twitter_post_2", 7)
                    }
                },
                {
                    ConversionPagevisitAmericasCoffeePoster,
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
                    ConversionUserregistration,
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
                    ConversionPagevisitColombia,
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
                    ConversionFormsubmissionTryFreeSample,
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
                    ConversionPagevisitAmericasCoffeePoster,
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
                    ConversionUserregistration,
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

        private readonly string _contactEmailPostfix;

        private readonly SiteInfo _mSite;

        private readonly Guid _newsletterCoffeeClubMembershipIssueGuid =
            Guid.Parse("ADCFDF3E-3AA3-4228-8627-5B29EEA018DB");

        private readonly Guid _newsletterColombiaCoffeeSamplePromotionIssueGuid =
            Guid.Parse("985750F8-A8E0-4704-800E-3CCD1198BEA7");

        public CampaignDataGenerator(SiteInfo site, string contactEmailPostfix)
        {
            _mSite = site;
            _contactEmailPostfix = contactEmailPostfix;
        }

        private static ActivityDataParameters GetActivityDataParameters(
            string utmSource,
            string utmContent,
            int count) =>
            new ActivityDataParameters
            {
                UtmSource = utmSource,
                UtmContent = utmContent,
                Count = count
            };

        public void Generate()
        {
            CampaignInfoProvider.GetCampaigns().WhereStartsWith("CampaignName", "DancingGoat.")
                .OnSite(_mSite.SiteID).ToList()
                .ForEach(CampaignInfoProvider.DeleteCampaignInfo);

            GenerateCoffeeClubMembershipCampaign();
            GenerateCafePromotionSampleCampaign();

            GenerateCampaignObjective(CampaignCafeSamplePromotionRunning, ConversionFormsubmissionTryFreeSample, 600);
            GenerateCampaignObjective(CampaignCafeSamplePromotionFinished, ConversionFormsubmissionTryFreeSample, 50);

            GenerateActivities(CampaignCafeSamplePromotionRunning, _campaignCafeSamplePromotionRunningHits, CampaignCafeSamplePromotionRunningContactsCount);
            GenerateActivities(CampaignCafeSamplePromotionFinished, _campaignCafeSamplePromotionFinishedHits, CampaignCafeSamplePromotionFinishedContactsCount);
            
            var result = new CalculateCampaignConversionReportTask().Execute(new TaskInfo
            {
                TaskSiteID = _mSite.SiteID
            });

            if (!string.IsNullOrEmpty(result))
            {
                throw new InvalidOperationException($"Exception message: '{result}'");
            }
        }
        
        private void GenerateCampaignObjective(
            string campaignName,
            string conversionName,
            int objectiveValue)
        {
            var campaignInfo = CampaignInfoProvider.GetCampaignInfo(campaignName, _mSite.SiteName);
            if (campaignInfo == null)
            {
                return;
            }

            var campaignConversionInfo = CampaignConversionInfoProvider.GetCampaignConversions()
                .WhereEquals("CampaignConversionDisplayName", conversionName)
                .WhereEquals("CampaignConversionCampaignID", campaignInfo.CampaignID)
                .FirstOrDefault();
            if (campaignConversionInfo == null)
            {
                return;
            }

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
            {
                return;
            }

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
            {
                CampaignDataGeneratorHelpers.AddPageAsset(campaignInfo.CampaignID, pagePath);
            }

            foreach (var conversionData in campaignData.CampaignReportSetup)
            {
                CampaignDataGeneratorHelpers.CreateConversion(campaignInfo.CampaignID, conversionData);
            }
        }

        private void GenerateCoffeeClubMembershipCampaign()
        {
            var campaignData = new CampaignData
            {
                CampaignName = CampaignCoffeeClubMembershipDraft,
                CampaignDisplayName = "Coffee club membership",
                CampaignDescription =
                    "The goal of this campaign is to promote the Coffee Club, a new service that the Dancing Goat company provides for it's coffee geek customers.",
                CampaignUtmCode = "coffee_club_membership_draft",
                CampaignOpenFrom = DateTime.MinValue,
                CampaignOpenTo = DateTime.MinValue,
                CampaignEmailPromotion = _newsletterCoffeeClubMembershipIssueGuid,
                CampaignContentInventory = new List<string>
                {
                    PagePathCoffeeClubMembership
                },
                CampaignReportSetup = PrepareCoffeeClubMembershipConversions()
            };
            GenerateCampaign(campaignData);
            campaignData.CampaignName = CampaignCoffeeClubMembershipScheduled;
            campaignData.CampaignDisplayName = "Coffee club membership test";
            campaignData.CampaignOpenFrom = DateTime.Now.AddDays(6.0);
            campaignData.CampaignUtmCode = "coffee_club_membership_scheduled";
            GenerateCampaign(campaignData);
        }

        private void GenerateCafePromotionSampleCampaign()
        {
            var campaignData = new CampaignData
            {
                CampaignName = CampaignCafeSamplePromotionRunning,
                CampaignDisplayName = "Cafe sample promotion",
                CampaignDescription =
                    "The goal of this campaign is to increase the number of visitors in our cafes. We want to achieve that by sending out free coffee sample coupons that customers can redeem at the cafes. At the end of the process a poster download is offered to see whether people would be interested in such freebies.",
                CampaignUtmCode = "cafe_sample_promotion_running",
                CampaignOpenFrom = DateTime.Now.AddDays(-14.0),
                CampaignOpenTo = DateTime.MinValue,
                CampaignEmailPromotion = _newsletterColombiaCoffeeSamplePromotionIssueGuid,
                CampaignContentInventory = new List<string>
                {
                    PagePathColombia,
                    PagePathThankYou,
                    PagePathAmericasCoffeePoster
                },
                CampaignReportSetup = PrepareCafeSamplePromotionConversions()
            };
            GenerateCampaign(campaignData);
            campaignData.CampaignName = CampaignCafeSamplePromotionFinished;
            campaignData.CampaignDisplayName = "Cafe sample promotion test";
            campaignData.CampaignOpenTo = campaignData.CampaignOpenFrom.AddDays(6.0);
            campaignData.CampaignUtmCode = "cafe_sample_promotion_finished";
            GenerateCampaign(campaignData);
        }

        private void GenerateActivities(
            string campaignName,
            IReadOnlyDictionary<string, IEnumerable<ActivityDataParameters>> conversionHits,
            int contactsCount)
        {
            var siteName = _mSite.SiteName;
            var campaignInfo = CampaignInfoProvider.GetCampaignInfo(campaignName, siteName);
            var posterPath = CampaignDataGeneratorHelpers.GetDocument(PagePathAmericasCoffeePoster);
            var colombiaPath = CampaignDataGeneratorHelpers.GetDocument(PagePathColombia);
            var bizFormInfo = BizFormInfoProvider.GetBizFormInfo(TryFreeSampleFormCodeName, _mSite.SiteID);
            CampaignDataGeneratorHelpers.DeleteOldActivities(campaignInfo.CampaignUTMCode);
            var contactsIDs = new ContactsIdData(_contactEmailPostfix, contactsCount);

            CampaignDataGeneratorHelpers.GenerateActivities(conversionHits[ConversionPagevisitColombia], campaignInfo,
                "pagevisit",
                contactsIDs, colombiaPath.NodeID);

            CampaignDataGeneratorHelpers.GenerateActivities(conversionHits[ConversionPagevisitAmericasCoffeePoster],
                campaignInfo,
                "pagevisit", contactsIDs, posterPath.NodeID);

            CampaignDataGeneratorHelpers.GenerateActivities(conversionHits[ConversionUserregistration], campaignInfo,
                "userregistration", contactsIDs);

            CampaignDataGeneratorHelpers.GenerateActivities(conversionHits[ConversionFormsubmissionTryFreeSample],
                campaignInfo,
                "bizformsubmit", contactsIDs, bizFormInfo.FormID);
        }

        private IEnumerable<CampaignConversionData> PrepareCafeSamplePromotionConversions()
        {
            var document1 =
                CampaignDataGeneratorHelpers.GetDocument(PagePathAmericasCoffeePoster);
            var document2 = CampaignDataGeneratorHelpers.GetDocument(PagePathColombia);
            var bizFormInfo = BizFormInfoProvider.GetBizFormInfo(TryFreeSampleFormCodeName, _mSite.SiteID);
            return new List<CampaignConversionData>
            {
                new CampaignConversionData
                {
                    ConversionName = "try_free_sample",
                    ConversionDisplayName = ConversionFormsubmissionTryFreeSample,
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
                    ConversionDisplayName = ConversionFormsubmissionTryFreeSample,
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

        private static IEnumerable<CampaignConversionData> PrepareCoffeeClubMembershipConversions()
        {
            var document = CampaignDataGeneratorHelpers.GetDocument(PagePathCoffeeClubMembership);
            var coffeeclubmembershipdocument = CampaignDataGeneratorHelpers.GetDocument("/Products/Brewers/AeroPress");
            return new List<CampaignConversionData>
            {
                new CampaignConversionData
                {
                    ConversionName = "coffee_club_membership",
                    ConversionDisplayName = document.DocumentName,
                    ConversionActivityType = "purchasedproduct",
                    ConversionItemId = coffeeclubmembershipdocument.NodeSKUID,
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
                    ConversionItemId = coffeeclubmembershipdocument.NodeSKUID,
                    ConversionOrder = 2,
                    ConversionIsFunnelStep = true
                },
                new CampaignConversionData
                {
                    ConversionName = "coffee_club_membership_3",
                    ConversionDisplayName = document.DocumentName,
                    ConversionActivityType = "purchasedproduct",
                    ConversionItemId = coffeeclubmembershipdocument.NodeSKUID,
                    ConversionOrder = 3,
                    ConversionIsFunnelStep = true
                }
            };
        }
    }
}