using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Activities;
using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.WebAnalytics;

namespace DancingGoat.Generator.WebAnalytics
{
    public static class CampaignDataGeneratorHelpers
    {
        public static void DeleteOldActivities(string campaignUtmCode)
        {
            ActivityInfoProvider.GetActivities().WhereStartsWith("ActivityTitle", "GeneratedActivity_")
                .WhereEquals("ActivityCampaign", campaignUtmCode).ToList()
                .ForEach(ActivityInfoProvider.DeleteActivityInfo);
        }

        public static void GenerateActivities(
            IEnumerable<ActivityDataParameters> activityDataParameters,
            CampaignInfo campaign,
            string type,
            ContactsIdData contactsIDs,
            int conversionItemId = 0)
        {
            var nodeId = 0;
            var itemId = 0;
            if (type == "pagevisit")
                nodeId = conversionItemId;
            else
                itemId = conversionItemId;
            foreach (var activityDataParameter in activityDataParameters)
                for (var index = 0; index < activityDataParameter.Count; ++index)
                    GenerateFakeActivity(campaign.CampaignUTMCode, type, activityDataParameter, nodeId, itemId,
                        campaign.CampaignSiteID, contactsIDs.GetNextContactId());
        }

        private static void GenerateFakeActivity(
            string campaignUtMcode,
            string type,
            ActivityDataParameters activityDataParameter,
            int nodeId,
            int itemId,
            int siteId,
            int contactId)
        {
            ActivityInfoProvider.SetActivityInfo(new ActivityInfo
            {
                ActivitySiteID = siteId,
                ActivityContactID = contactId,
                ActivityCampaign = campaignUtMcode,
                ActivityType = type,
                ActivityNodeID = nodeId,
                ActivityItemID = itemId,
                ActivityUTMSource = activityDataParameter.UtmSource,
                ActivityUTMContent = activityDataParameter.UtmContent,
                ActivityTitle = "GeneratedActivity_" + type + "_" + contactId
            });
        }

        public static void CreateConversion(int campaignId, CampaignConversionData conversionData)
        {
            if (CampaignConversionInfoProvider.GetCampaignConversions()
                    .WhereEquals("CampaignConversionCampaignID", campaignId)
                    .WhereEquals("CampaignConversionActivityType", conversionData.ConversionActivityType)
                    .WhereEquals("CampaignConversionItemID", conversionData.ConversionItemId)
                    .WhereEquals("CampaignConversionIsFunnelStep", conversionData.ConversionIsFunnelStep).ToList()
                    .FirstOrDefault() != null)
                return;
            CampaignConversionInfoProvider.SetCampaignConversionInfo(new CampaignConversionInfo
            {
                CampaignConversionName = conversionData.ConversionName,
                CampaignConversionDisplayName = conversionData.ConversionDisplayName,
                CampaignConversionCampaignID = campaignId,
                CampaignConversionActivityType = conversionData.ConversionActivityType,
                CampaignConversionItemID = conversionData.ConversionItemId.GetValueOrDefault(),
                CampaignConversionIsFunnelStep = conversionData.ConversionIsFunnelStep,
                CampaignConversionOrder = conversionData.ConversionOrder
            });
        }

        public static void AddNewsletterAsset(CampaignInfo campaign, Guid issueGuid)
        {
            var infoByGuid = ProviderHelper.GetInfoByGuid("newsletter.issue", issueGuid, campaign.CampaignSiteID);
            if (infoByGuid == null)
                return;
            var infoById = ProviderHelper.GetInfoById("newsletter.newsletter",
                infoByGuid.GetValue("IssueNewsletterID").ToInteger(0));
            if (infoById == null)
                return;
            var lowerInvariant = infoById.GetValue("NewsletterDisplayName").ToString().Replace(' ', '_')
                .ToLowerInvariant();
            infoByGuid.SetValue("IssueUseUTM", true);
            infoByGuid.SetValue("IssueUTMCampaign", campaign.CampaignUTMCode);
            infoByGuid.SetValue("IssueUTMSource", lowerInvariant);
            infoByGuid.Update();
            CreateNewsletterAsset(campaign.CampaignID, issueGuid);
        }

        private static void CreateNewsletterAsset(int campaignId, Guid nodeGuid)
        {
            if (CampaignAssetInfoProvider.GetCampaignAssets().WhereEquals("CampaignAssetCampaignID", campaignId)
                    .WhereEquals("CampaignAssetAssetGuid", nodeGuid).ToList().FirstOrDefault() != null)
                return;
            CampaignAssetInfoProvider.SetCampaignAssetInfo(new CampaignAssetInfo
            {
                CampaignAssetType = "newsletter.issue",
                CampaignAssetCampaignID = campaignId,
                CampaignAssetAssetGuid = nodeGuid
            });
        }

        public static void AddPageAsset(int campaignId, string pagePath)
        {
            var nodeGuid = GetDocument(pagePath).NodeGUID;
            if (CampaignAssetInfoProvider.GetCampaignAssets().WhereEquals("CampaignAssetCampaignID", campaignId)
                    .WhereEquals("CampaignAssetAssetGuid", nodeGuid).ToList().FirstOrDefault() != null)
                return;
            CampaignAssetInfoProvider.SetCampaignAssetInfo(new CampaignAssetInfo
            {
                CampaignAssetType = "cms.document",
                CampaignAssetCampaignID = campaignId,
                CampaignAssetAssetGuid = nodeGuid
            });
        }

        public static TreeNode GetDocument(string path)
        {
            return DocumentHelper.GetDocuments().All().Culture("en-US").Path(path).OnCurrentSite().ToList().First();
        }
    }
}