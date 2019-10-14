using System.Linq;
using CMS.ContactManagement;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.SiteProvider;

namespace DancingGoat.Generator.WebAnalytics
{
    public class ScoringWithRulesGenerator
    {
        private readonly SiteInfo _mSite;

        /// <summary>Constructor.</summary>
        /// <param name="site">Site that will contain generated objects</param>
        public ScoringWithRulesGenerator(SiteInfo site)
        {
            _mSite = site;
        }

        /// <summary>Performs scoring objects and rules generation.</summary>
        public void Generate()
        {
            if (ScoreInfoProvider.GetScores().WhereEquals("ScoreName", "EngagementAndBusinessFit")
                    .WhereFalse("ScoreBelongsToPersona").TopN(1).FirstOrDefault() != null)
                return;
            var scoreObj = new ScoreInfo
            {
                ScoreDisplayName = "Engagement and business fit",
                ScoreName = "EngagementAndBusinessFit",
                ScoreDescription =
                    "Measures the fit and interest of B2B prospects on the site. Fit is measured by demographics and geographics. Interest is measured by behavior on the site that can be tied to B2B activities, such as visiting the 'Partnership' section of the site or providing a phone number.",
                ScoreEnabled = true,
                ScoreStatus = ScoreStatusEnum.RecalculationRequired,
                ScoreEmailAtScore = 20,
                ScoreNotificationEmail = "sales@localhost.local"
            };
            ScoreInfoProvider.SetScoreInfo(scoreObj);
            var treeNode = DocumentHelper.GetDocuments().All().Culture("en-US").Path("/Partnership").Columns("NodeID")
                .OnCurrentSite().TopN(1).FirstOrDefault();
            if (treeNode != null)
            {
                var rule = GenerateRule("Visited the Partnership section", 5, scoreObj.ScoreID,
                    "<condition>\r\n  <activity name=\"pagevisit\">\r\n    <field name=\"ActivityCreated\">\r\n      <settings>\r\n        <seconddatetime>1/1/0001 12:00:00 AM</seconddatetime>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityNodeID\">\r\n      <value>" +
                    treeNode.NodeID +
                    "</value>\r\n    </field>\r\n    <field name=\"ActivityURL\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityTitle\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityComment\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityCampaign\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityURLReferrer\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityABVariantName\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityMVTCombinationName\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n  </activity>\r\n  <wherecondition>(ActivityType='pagevisit') AND ([ActivityNodeID] = " +
                    treeNode.NodeID + ")</wherecondition>\r\n</condition>", RuleTypeEnum.Activity, "pagevisit", false);
                rule.RuleIsRecurring = true;
                rule.RuleMaxPoints = 15;
                RuleInfoProvider.SetRuleInfo(rule);
            }

            var bizFormInfo = BizFormInfoProvider.GetBizFormInfo("BusinessCustomerRegistration", _mSite.SiteID);
            if (bizFormInfo != null)
                GenerateRule("Submitted the business registration form", 15, scoreObj.ScoreID,
                    BuildMacroRuleCondition("{%Rule(\"(Contact.SubmittedForm(\\\"" + bizFormInfo.FormName +
                                            "\\\", ToInt(0)))\", \"&lt;rules&gt;&lt;r pos=\\\"0\\\" par=\\\"\\\" op=\\\"and\\\" n=\\\"CMSContactHasSubmittedSpecifiedFormInLastXDays\\\" &gt;&lt;p n=\\\"_perfectum\\\"&gt;&lt;t&gt;has&lt;/t&gt;&lt;v&gt;&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;select operation&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"days\\\"&gt;&lt;t&gt;#enter days&lt;/t&gt;&lt;v&gt;0&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;enter days&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"item\\\"&gt;&lt;t&gt;" +
                                            bizFormInfo.FormName + "&lt;/t&gt;&lt;v&gt;" + bizFormInfo.FormName +
                                            "&lt;/v&gt;&lt;r&gt;1&lt;/r&gt;&lt;d&gt;select form&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;\")%}"),
                    RuleTypeEnum.Macro, null, false);
            GenerateRule("Provided phone number", 10, scoreObj.ScoreID,
                "<condition>\r\n  <attribute name=\"ContactBusinessPhone\">\r\n    <params>\r\n      <ContactBusinessPhoneOperator>9</ContactBusinessPhoneOperator>\r\n    </params>\r\n  </attribute>\r\n  <wherecondition>([ContactBusinessPhone] &lt;&gt; N'' AND [ContactBusinessPhone] IS NOT NULL)</wherecondition>\r\n</condition>",
                RuleTypeEnum.Attribute, "ContactBusinessPhone");
            RecalculateScores();
        }

        private string BuildMacroRuleCondition(string macroCondition)
        {
            return "<condition>\r\n  <macro>\r\n    <value>" + MacroSecurityProcessor.AddSecurityParameters(
                       macroCondition, MacroIdentityOption.FromUserInfo(UserInfoProvider.AdministratorUser),
                       null) +
                   "</value>\r\n  </macro>\r\n</condition>";
        }

        private RuleInfo GenerateRule(
            string displayName,
            int value,
            int scoreId,
            string ruleCondition,
            RuleTypeEnum ruleType,
            string ruleParameter = null,
            bool belongsToPersona = true)
        {
            var ruleObj = new RuleInfo();
            ruleObj.RuleScoreID = scoreId;
            ruleObj.RuleDisplayName = displayName;
            ruleObj.RuleName = ValidationHelper.GetCodeName(displayName, 100);
            ruleObj.RuleValue = value;
            ruleObj.RuleType = ruleType;
            ruleObj.RuleParameter = ruleParameter;
            ruleObj.RuleCondition = ruleCondition;
            ruleObj.RuleBelongsToPersona = belongsToPersona;
            RuleInfoProvider.SetRuleInfo(ruleObj);
            return ruleObj;
        }

        private void RecalculateScores()
        {
            foreach (var score in ScoreInfoProvider.GetScores()
                .WhereEquals("ScoreStatus", ScoreStatusEnum.RecalculationRequired).WhereFalse("ScoreBelongsToPersona"))
                new ScoreAsyncRecalculator(score).RunAsync();
        }
    }
}