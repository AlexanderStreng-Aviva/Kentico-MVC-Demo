using System;
using System.Linq;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.OnlineMarketing;
using CMS.SiteProvider;
using CMS.WebAnalytics;

namespace DancingGoat.Generator.WebAnalytics
{
    public class AbTestAndConversionDataGenerator
    {
        private const string CoffeeSampleOrderConversionName = "CoffeeSampleOrder";
        private const string ColombiaLandingPageAbTestName = "ColombiaLandingPageA_BTest";
        private const string ColombiaLandingPageOriginalVariantName = "Original";
        private const string ColombiaLandingPageBVariantName = "BVariant";
        private const string CafeSamplePromotionCampaignName = "CafeSamplePromotion";
        private const int ConversionValue = 10;

        private readonly int[] _mConversionsForBPage = new int[15]
        {
            35,
            45,
            40,
            30,
            16,
            9,
            5,
            5,
            4,
            2,
            3,
            1,
            1,
            0,
            0
        };

        private readonly int[] _mConversionsForOriginalPage = new int[15]
        {
            58,
            71,
            62,
            43,
            28,
            17,
            12,
            7,
            4,
            2,
            2,
            1,
            1,
            2,
            0
        };

        private readonly int[] _mFirstConversionsForBPage = new int[15]
        {
            35,
            43,
            38,
            27,
            15,
            9,
            5,
            4,
            4,
            2,
            1,
            1,
            1,
            0,
            0
        };

        private readonly int[] _mFirstConversionsForOriginalPage = new int[15]
        {
            57,
            69,
            60,
            42,
            28,
            17,
            11,
            5,
            3,
            2,
            2,
            1,
            1,
            1,
            0
        };

        private readonly int[] _mFirstVisitorsForBPage = new int[15]
        {
            66,
            75,
            72,
            52,
            44,
            37,
            21,
            13,
            9,
            4,
            4,
            4,
            3,
            2,
            2
        };

        private readonly int[] _mFirstVisitorsForOriginalPage = new int[15]
        {
            65,
            76,
            72,
            52,
            43,
            38,
            22,
            12,
            8,
            5,
            5,
            3,
            3,
            2,
            1
        };

        private readonly int[] _mRecurringConversionsForBPage = new int[15]
        {
            35,
            43,
            38,
            27,
            15,
            9,
            5,
            4,
            4,
            2,
            1,
            1,
            1,
            0,
            0
        };

        private readonly int[] _mRecurringConversionsForOriginalPage = new int[15]
        {
            1,
            2,
            2,
            1,
            0,
            0,
            1,
            2,
            1,
            0,
            0,
            0,
            0,
            1,
            0
        };

        private readonly int[] _mReturningVisitorsForBPage = new int[15]
        {
            18,
            22,
            19,
            22,
            17,
            17,
            15,
            19,
            20,
            26,
            24,
            26,
            29,
            31,
            30
        };

        private readonly int[] _mReturningVisitorsForOriginalPage = new int[15]
        {
            17,
            19,
            23,
            25,
            19,
            15,
            15,
            17,
            18,
            20,
            26,
            22,
            33,
            35,
            33
        };

        private readonly SiteInfo _mSite;
        private ABTestInfo _mAbTest;
        private ABVariantInfo _mBVariant;
        private CampaignInfo _mCampaign;
        private ConversionInfo _mConversion;
        private ABVariantInfo _mOriginalVariant;

        /// <summary>Constructor.</summary>
        /// <param name="site">Site to generate data for</param>
        public AbTestAndConversionDataGenerator(SiteInfo site)
        {
            _mSite = site;
        }

        private ABTestInfo AbTest => _mAbTest ?? (_mAbTest = ABTestInfoProvider.GetABTests().TopN(1)
                                         .OnSite(_mSite.SiteID)
                                         .WhereEquals("ABTestName", "ColombiaLandingPageA_BTest").FirstOrDefault());

        private ConversionInfo Conversion => _mConversion ?? (_mConversion = ConversionInfoProvider.GetConversions()
                                                 .TopN(1).OnSite(_mSite.SiteID)
                                                 .WhereEquals("ConversionName", "CoffeeSampleOrder").FirstOrDefault());

        private ABVariantInfo OriginalVariant
        {
            get
            {
                if (_mOriginalVariant == null)
                    _mOriginalVariant = ABCachedObjects.GetVariants(AbTest)
                        .Single(variant => variant.ABVariantName == "Original");
                return _mOriginalVariant;
            }
        }

        private ABVariantInfo BVariant
        {
            get
            {
                if (_mBVariant == null)
                    _mBVariant = ABCachedObjects.GetVariants(AbTest)
                        .Single(variant => variant.ABVariantName == nameof(BVariant));
                return _mBVariant;
            }
        }

        private CampaignInfo Campaign => _mCampaign ?? (_mCampaign = CampaignInfoProvider.GetCampaigns().TopN(1)
                                             .OnSite(_mSite.SiteID).WhereEquals("CampaignName", "CafeSamplePromotion")
                                             .FirstOrDefault());

        /// <summary>
        ///     Generates statistics for AB tests on the current site.
        /// </summary>
        public void Generate()
        {
            if (AbTest == null || Conversion == null || Campaign == null)
                return;
            var now = DateTime.Now;
            var dateTime = now.AddDays(-14.0);
            AbTest.ABTestOpenFrom = dateTime;
            AbTest.ABTestOpenTo = now;
            ClearStatisticsData();
            for (var logDate = dateTime; logDate < now.AddDays(1.0); logDate = logDate.AddDays(1.0))
            {
                var days = (logDate - dateTime).Days;
                LogAbConversionHits(days, logDate);
                LogAbVisitHits(days, logDate);
            }

            AbTest.ABTestWinnerGUID = OriginalVariant.ABVariantGUID;
            ABTestInfoProvider.SetABTestInfo(AbTest);
        }

        /// <summary>
        ///     Removes all previously logged data related to the Dancing Goat default A/B test and Campaign.
        /// </summary>
        private void ClearStatisticsData()
        {
            var whereCondition = new WhereCondition().WhereContains("StatisticsCode", AbTest.ABTestName).Or()
                .WhereEquals("StatisticsObjectName", Campaign.CampaignUTMCode).Or()
                .WhereEquals("StatisticsObjectName", Conversion.ConversionName);
            StatisticsInfoProvider.RemoveAnalyticsData(DateTimeHelper.ZERO_TIME, DateTimeHelper.ZERO_TIME,
                _mSite.SiteID,
                whereCondition.ToString(true));
        }

        /// <summary>Performs logging of A/B conversion hits.</summary>
        /// <param name="daysFromStart">
        ///     Determines for which day should be the logging performed (selects correct values from the
        ///     data arrays)
        /// </param>
        /// <param name="logDate">Date the hit is assigned to</param>
        private void LogAbConversionHits(int daysFromStart, DateTime logDate)
        {
            var action = (Action<string, int[], int[]>) ((type, originalVariantData, bVariantData) =>
            {
                LogHit(GetAbHitCodename(type, AbTest, OriginalVariant), originalVariantData[daysFromStart],
                    originalVariantData[daysFromStart] * 10, logDate, Conversion.ConversionName);
                LogHit(GetAbHitCodename(type, AbTest, BVariant), bVariantData[daysFromStart],
                    bVariantData[daysFromStart] * 10, logDate, Conversion.ConversionName);
            });
            action("absessionconversionfirst", _mFirstConversionsForOriginalPage, _mFirstConversionsForBPage);
            action("absessionconversionrecurring", _mRecurringConversionsForOriginalPage,
                _mRecurringConversionsForBPage);
            action("abconversion", _mConversionsForOriginalPage, _mConversionsForBPage);
        }

        /// <summary>Performs logging of A/B visit hits.</summary>
        /// <param name="daysFromStart">
        ///     Determines for which day should be the logging performed (selects correct values from the
        ///     data arrays)
        /// </param>
        /// <param name="logDate">Date the hit is assigned to</param>
        private void LogAbVisitHits(int daysFromStart, DateTime logDate)
        {
            var action = (Action<string, int[], int[]>) ((type, originalVariantData, bVariantData) =>
            {
                LogHit(GetAbHitCodename(type, AbTest, OriginalVariant), originalVariantData[daysFromStart], 0, logDate,
                    OriginalVariant.ABVariantPath);
                LogHit(GetAbHitCodename(type, AbTest, BVariant), bVariantData[daysFromStart], 0, logDate,
                    BVariant.ABVariantPath);
            });
            action("abvisitfirst", _mFirstVisitorsForOriginalPage, _mFirstVisitorsForBPage);
            action("abvisitreturn", _mReturningVisitorsForOriginalPage, _mReturningVisitorsForBPage);
        }

        /// <summary>
        ///     Constructs proper statistics code name for the A/B hit.
        /// </summary>
        private string GetAbHitCodename(
            string statisticsType,
            ABTestInfo abTest,
            ABVariantInfo variant)
        {
            return statisticsType + ";" + abTest.ABTestName + ";" + variant.ABVariantName;
        }

        /// <summary>Performs logging of the hit.</summary>
        private void LogHit(
            string codeName,
            int visits,
            int value,
            DateTime logTime,
            string objectName)
        {
            HitLogProcessor.SaveLogToDatabase(new LogRecord
            {
                CodeName = codeName,
                Hits = visits,
                Value = value,
                LogTime = logTime,
                ObjectName = objectName,
                SiteName = _mSite.SiteName,
                Culture = "en-US"
            });
        }
    }
}