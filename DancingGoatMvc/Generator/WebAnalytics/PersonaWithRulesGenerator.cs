using System;
using System.Collections.ObjectModel;
using System.Linq;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Newsletters;
using CMS.OnlineMarketing;
using CMS.Personas;
using CMS.SiteProvider;

namespace DancingGoat.Generator.WebAnalytics
{
    public class PersonaWithRulesGenerator
    {
        private const string CoffeeGeekPersonaContactGroupName = "IsInPersona_Martina_TheCoffeeGeek";
        public const string PERSONA_CAFE_OWNER = "Tony_The_Cafe_Owner";
        public const string PERSONA_COFEE_GEEK = "Martina_The_Cofee_Geek";
        private readonly Random _mRandom = new Random();
        private readonly SiteInfo _mSite;

        public PersonaWithRulesGenerator(SiteInfo site)
        {
            _mSite = site;
        }

        public void Generate()
        {
            GenerateCoffeeGeekPersona();
            GenerateCafeOwnerPersona();
            AssignPersonaToSkUs(PERSONA_CAFE_OWNER, new[]
            {
                "Hario Vacuum Pot",
                "Macap M4D",
                "Guatemala Finca El Injerto",
                "Nicaragua Dipilto"
            });
            AssignPersonaToSkUs(PERSONA_COFEE_GEEK, new[]
            {
                "Anfim Super Caimano",
                "Espro Press",
                "Hario Buono Kettle",
                "AeroPress"
            });
            RecalculatePersonas();
            GenerateBannerPersonalizationVariantsMacrosAndEnableVariants();
            GeneratePersonaContactHistory();
        }

        private void GenerateCoffeeGeekPersona()
        {
            var personaInfo = PersonaInfoProvider.GetPersonaInfoByCodeName(PERSONA_COFEE_GEEK);
            if (personaInfo == null)
            {
                personaInfo = new PersonaInfo
                {
                    PersonaDisplayName = "Martina, the Coffee Geek",
                    PersonaName = PERSONA_COFEE_GEEK,
                    PersonaDescription =
                        "Martina is 28, she's an online entrepreneur and a foodie girl who likes to blog about her gastronomic experiences. \r\n\r\nThe preparation of her coffee has to be perfect.  She knows all the technical bits that go into the process. Not to leave things to chance, she also owns a professional espresso machine and a grinder.\r\n\r\nMartina drinks a cappuccino or a filtered coffee in the morning and then an espresso or machiato after each meal.",
                    PersonaPointsThreshold = 15,
                    PersonaPictureMetafileGUID = new Guid("8A3AF6F7-0914-42E1-9641-B7F2E04AED1B"),
                    PersonaEnabled = true
                };
                PersonaInfoProvider.SetPersonaInfo(personaInfo);
            }

            SubscribeCoffeeGeekContactGroupToEmailCampaign(personaInfo);
            var newsletterInfo = NewsletterInfoProvider.GetNewsletterInfo("DancingGoatNewsletter", _mSite.SiteID);
            if (newsletterInfo != null)
            {
                GenerateRule("Is subscribed to the Dancing goat newsletter", 10, personaInfo.PersonaScoreID,
                    "<condition>\r\n  <activity name=\"newslettersubscription\">\r\n    <field name=\"ActivityItemID\">\r\n      <value>" +
                    newsletterInfo.NewsletterID +
                    "</value>\r\n    </field>\r\n    <field name=\"ActivityCreated\">\r\n      <settings>\r\n        <seconddatetime>1/1/0001 12:00:00 AM</seconddatetime>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityURL\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityTitle\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityComment\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityCampaign\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityURLReferrer\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n  </activity>\r\n  <wherecondition>(ActivityType='newslettersubscription') AND ([ActivityItemID] = " +
                    newsletterInfo.NewsletterID + ")</wherecondition>\r\n</condition>", RuleTypeEnum.Activity,
                    "newslettersubscription");
            }

            GenerateRule("Downloaded the America's coffee poster file", 5, personaInfo.PersonaScoreID,
                BuildMacroRuleCondition(
                    "<condition>\r\n  <macro>\r\n    <value>{%Rule(\"(Contact.VisitedPage(\\\"/Campaign-assets/Cafe-promotion/America-s-coffee-poster\\\", ToInt(0)))\", \"&lt;rules&gt;&lt;r pos=\\\"0\\\" par=\\\"\\\" op=\\\"and\\\" n=\\\"CMSContactHasDownloadedSpecifiedFileInLastXDays\\\" &gt;&lt;p n=\\\"_perfectum\\\"&gt;&lt;t&gt;has&lt;/t&gt;&lt;v&gt;&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;select operation&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"days\\\"&gt;&lt;t&gt;#enter days&lt;/t&gt;&lt;v&gt;0&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;enter days&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"item\\\"&gt;&lt;t&gt;/Campaign-assets/Cafe-promotion/America-s-coffee-poster&lt;/t&gt;&lt;v&gt;/Campaign-assets/Cafe-promotion/America-s-coffee-poster&lt;/v&gt;&lt;r&gt;1&lt;/r&gt;&lt;d&gt;select file&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;1&lt;/tv&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;\")%}</value>\r\n  </macro>\r\n</condition>"),
                RuleTypeEnum.Macro);
            GenerateRule("Spent between $1 and $100", 10, personaInfo.PersonaScoreID,
                BuildMacroRuleCondition(
                    "{%Rule(\"(Contact.SpentMoney(ToDouble(1), ToDouble(100), ToInt(90)))\", \"&lt;rules&gt;&lt;r pos=\\\"0\\\" par=\\\"\\\" op=\\\"and\\\" n=\\\"CMSContactHasSpentMoneyInTheStoreInTheLastXDays\\\" &gt;&lt;p n=\\\"money1\\\"&gt;&lt;t&gt;1&lt;/t&gt;&lt;v&gt;1&lt;/v&gt;&lt;r&gt;1&lt;/r&gt;&lt;d&gt;enter value&lt;/d&gt;&lt;vt&gt;double&lt;/vt&gt;&lt;tv&gt;1&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"days\\\"&gt;&lt;t&gt;90&lt;/t&gt;&lt;v&gt;90&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;enter days&lt;/d&gt;&lt;vt&gt;integer&lt;/vt&gt;&lt;tv&gt;1&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"money2\\\"&gt;&lt;t&gt;100&lt;/t&gt;&lt;v&gt;100&lt;/v&gt;&lt;r&gt;1&lt;/r&gt;&lt;d&gt;enter value&lt;/d&gt;&lt;vt&gt;double&lt;/vt&gt;&lt;tv&gt;1&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"_perfectum\\\"&gt;&lt;t&gt;has&lt;/t&gt;&lt;v&gt;&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;select operation&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;\")%}"),
                RuleTypeEnum.Macro);
        }

        private static void SubscribeCoffeeGeekContactGroupToEmailCampaign(PersonaInfo persona)
        {
            var issueInfo = IssueInfoProvider.GetIssues().WhereIn("IssueNewsletterID",
                NewsletterInfoProvider.GetNewsletters().WhereEquals("NewsletterName", "CoffeeClubMembership")
                    .Column("NewsletterID")).TopN(1).FirstOrDefault();
            if (issueInfo == null)
            {
                return;
            }

            var contactGroup = CreateContactGroup(persona);
            if (IssueContactGroupInfoProvider.GetIssueContactGroupInfo(issueInfo.IssueID,
                    contactGroup.ContactGroupID) != null)
            {
                return;
            }

            IssueContactGroupInfoProvider.SetIssueContactGroupInfo(new IssueContactGroupInfo
            {
                IssueID = issueInfo.IssueID,
                ContactGroupID = contactGroup.ContactGroupID
            });
        }

        private static ContactGroupInfo CreateContactGroup(PersonaInfo persona)
        {
            var contactGroupInfo = ContactGroupInfoProvider.GetContactGroupInfo(CoffeeGeekPersonaContactGroupName);
            if (contactGroupInfo != null)
            {
                ContactGroupInfoProvider.DeleteContactGroupInfo(contactGroupInfo);
            }

            var groupObj = new ContactGroupInfo();
            groupObj.ContactGroupDisplayName = "Is in persona 'Martina, the Coffee Geek'";
            groupObj.ContactGroupName = CoffeeGeekPersonaContactGroupName;
            groupObj.ContactGroupEnabled = true;
            var str = MacroSecurityProcessor.AddSecurityParameters(
                string.Format(
                    "{{%Rule(\"(Contact.IsInPersona(\\\"{0}\\\"))\", \"<rules><r pos=\\\"0\\\" par=\\\"\\\" op=\\\"and\\\" n=\\\"ContactIsInPersona\\\" ><p n=\\\"_is\\\"><t>is</t><v></v><r>0</r><d>select operation</d><vt>text</vt><tv>0</tv></p><p n=\\\"personaguid\\\"><t>{1}</t><v>{0}</v><r>1</r><d>select persona</d><vt>text</vt><tv>0</tv></p></r></rules>\") %}}",
                    persona.PersonaGUID, persona.PersonaDisplayName),
                MacroIdentityOption.FromUserInfo(UserInfoProvider.AdministratorUser), null);
            groupObj.ContactGroupDynamicCondition = str;
            ContactGroupInfoProvider.SetContactGroupInfo(groupObj);
            return groupObj;
        }

        private void GenerateCafeOwnerPersona()
        {
            var infoObj = PersonaInfoProvider.GetPersonaInfoByCodeName(PERSONA_CAFE_OWNER);
            if (infoObj == null)
            {
                infoObj = new PersonaInfo
                {
                    PersonaDisplayName = "Tony, the Cafe Owner",
                    PersonaName = PERSONA_CAFE_OWNER,
                    PersonaDescription =
                        "Tony has been running his own cafe for the last 7 years. He always looks at ways of improving the service he provides.\r\n\r\nHe offers coffee that he sources from several roasteries. In addition to that, he also sells brewing machines, accessories and grinders for home use.",
                    PersonaPointsThreshold = 15,
                    PersonaPictureMetafileGUID = new Guid("220C65BA-2CED-4347-9615-8CF69EAC20E5"),
                    PersonaEnabled = true
                };
                PersonaInfoProvider.SetPersonaInfo(infoObj);
            }

            var treeNode = DocumentHelper.GetDocuments().All().Culture("en-US").Path("/Partnership").Columns("NodeID")
                .OnCurrentSite().TopN(1).FirstOrDefault();
            if (treeNode != null)
            {
                var rule = GenerateRule("Visited the Partnership section", 5, infoObj.PersonaScoreID,
                    "<condition>\r\n  <activity name=\"pagevisit\">\r\n    <field name=\"ActivityCreated\">\r\n      <settings>\r\n        <seconddatetime>1/1/0001 12:00:00 AM</seconddatetime>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityNodeID\">\r\n      <value>" +
                    treeNode.NodeID +
                    "</value>\r\n    </field>\r\n    <field name=\"ActivityURL\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityTitle\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityComment\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityCampaign\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityURLReferrer\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityABVariantName\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n    <field name=\"ActivityMVTCombinationName\">\r\n      <settings>\r\n        <operator>0</operator>\r\n      </settings>\r\n    </field>\r\n  </activity>\r\n  <wherecondition>(ActivityType='pagevisit') AND ([ActivityNodeID] = " +
                    treeNode.NodeID + ")</wherecondition>\r\n</condition>", RuleTypeEnum.Activity, "pagevisit");
                rule.RuleIsRecurring = true;
                rule.RuleMaxPoints = 15;
                RuleInfoProvider.SetRuleInfo(rule);
            }

            var infoByName = ProviderHelper.GetInfoByName("cms.form", "BusinessCustomerRegistration", _mSite.SiteID);
            if (infoByName != null)
            {
                var obj = infoByName.GetValue("FormName");
                GenerateRule("Submitted the business registration form", 15, infoObj.PersonaScoreID,
                    BuildMacroRuleCondition("{%Rule(\"(Contact.SubmittedForm(\\\"" + obj +
                                            "\\\", ToInt(0)))\", \"&lt;rules&gt;&lt;r pos=\\\"0\\\" par=\\\"\\\" op=\\\"and\\\" n=\\\"CMSContactHasSubmittedSpecifiedFormInLastXDays\\\" &gt;&lt;p n=\\\"_perfectum\\\"&gt;&lt;t&gt;has&lt;/t&gt;&lt;v&gt;&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;select operation&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"days\\\"&gt;&lt;t&gt;#enter days&lt;/t&gt;&lt;v&gt;0&lt;/v&gt;&lt;r&gt;0&lt;/r&gt;&lt;d&gt;enter days&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;p n=\\\"item\\\"&gt;&lt;t&gt;" +
                                            obj + "&lt;/t&gt;&lt;v&gt;" + obj +
                                            "&lt;/v&gt;&lt;r&gt;1&lt;/r&gt;&lt;d&gt;select form&lt;/d&gt;&lt;vt&gt;text&lt;/vt&gt;&lt;tv&gt;0&lt;/tv&gt;&lt;/p&gt;&lt;/r&gt;&lt;/rules&gt;\")%}"),
                    RuleTypeEnum.Macro, null, false);
            }

            GenerateRule("Gmail penalization", -10, infoObj.PersonaScoreID,
                "<condition>\r\n  <attribute name=\"ContactEmail\">\r\n    <value>gmail.com</value>\r\n    <params>\r\n      <ContactEmailOperator>6</ContactEmailOperator>\r\n    </params>\r\n  </attribute>\r\n  <wherecondition>[ContactEmail] LIKE N'%gmail.com'</wherecondition>\r\n</condition>",
                RuleTypeEnum.Attribute, "ContactEmail");
        }

        private string BuildMacroRuleCondition(string macroCondition) =>
            "<condition>\r\n  <macro>\r\n    <value>" +
            MacroSecurityProcessor.AddSecurityParameters(macroCondition,
                MacroIdentityOption.FromUserInfo(UserInfoProvider.AdministratorUser), null) +
            "</value>\r\n  </macro>\r\n</condition>";

        private RuleInfo GenerateRule(
            string displayName,
            int value,
            int scoreId,
            string ruleCondition,
            RuleTypeEnum ruleType,
            string ruleParameter = null,
            bool belongsToPersona = true)
        {
            var codeName = ValidationHelper.GetCodeName(displayName, 100);
            var ruleInfo = RuleInfoProvider.GetRules().WithCodeName(codeName).FirstOrDefault();
            if (ruleInfo != null)
            {
                return ruleInfo;
            }

            var ruleObj = new RuleInfo
            {
                RuleScoreID = scoreId,
                RuleDisplayName = displayName,
                RuleName = codeName,
                RuleValue = value,
                RuleType = ruleType,
                RuleParameter = ruleParameter,
                RuleCondition = ruleCondition,
                RuleBelongsToPersona = belongsToPersona
            };
            RuleInfoProvider.SetRuleInfo(ruleObj);
            return ruleObj;
        }

        private static void AssignPersonaToSkUs(string personaName, string[] skuNames)
        {
            var personaInfoByCodeName = PersonaInfoProvider.GetPersonaInfoByCodeName(personaName);
            if (personaInfoByCodeName == null)
            {
                return;
            }

            foreach (var treeNode in DocumentHelper.GetDocuments().All().AllCultures().WhereIn("SKUName", skuNames)
                .Columns("NodeID").OnCurrentSite())
            {
                PersonaNodeInfoProvider.RemovePersonaFromNode(personaInfoByCodeName.PersonaID, treeNode.NodeID);
                PersonaNodeInfoProvider.AddPersonaToNode(personaInfoByCodeName.PersonaID, treeNode.NodeID);
            }
        }

        private void RecalculatePersonas()
        {
            foreach (var score in ScoreInfoProvider.GetScores()
                .WhereEquals("ScoreStatus", ScoreStatusEnum.RecalculationRequired).WhereTrue("ScoreBelongsToPersona"))
            {
                new ScoreAsyncRecalculator(score).RunAsync();
            }
        }

        private void GenerateBannerPersonalizationVariantsMacrosAndEnableVariants()
        {
            var personalizationVariant1 =
                ContentPersonalizationVariantInfoProvider.GetContentPersonalizationVariant(
                    "TheCafeOwnerBannerVariant_dd20f145-3779-4748-9bc8-d383eecfbd15");
            var personalizationVariant2 =
                ContentPersonalizationVariantInfoProvider.GetContentPersonalizationVariant(
                    "TheCoffeeGeekBannerVariant_d862b458-e5a5-432f-a3fe-60f495acad9f");
            if (personalizationVariant1 == null || personalizationVariant2 == null ||
                personalizationVariant1.VariantEnabled || personalizationVariant2.VariantEnabled ||
                personalizationVariant1.VariantDisplayCondition != "{%// Macro will be added by generator%}" ||
                personalizationVariant2.VariantDisplayCondition != "{%// Macro will be added by generator%}")
            {
                return;
            }

            var personaInfoByCodeName1 = PersonaInfoProvider.GetPersonaInfoByCodeName(PERSONA_CAFE_OWNER);
            var personaInfoByCodeName2 = PersonaInfoProvider.GetPersonaInfoByCodeName(PERSONA_COFEE_GEEK);
            if (personaInfoByCodeName1 == null || personaInfoByCodeName2 == null)
            {
                return;
            }

            var identityOption = MacroIdentityOption.FromUserInfo(UserInfoProvider.AdministratorUser);
            personalizationVariant1.VariantDisplayCondition = MacroSecurityProcessor.AddSecurityParameters(
                string.Format(
                    "{{%Rule(\"(Contact.IsInPersona(\\\"{0}\\\"))\", \"<rules><r pos=\\\"0\\\" par=\\\"\\\" op=\\\"and\\\" n=\\\"ContactIsInPersona\\\" ><p n=\\\"_is\\\"><t>is</t><v></v><r>0</r><d>select operation</d><vt>text</vt><tv>0</tv></p><p n=\\\"personaguid\\\"><t>{1}</t><v>{0}</v><r>1</r><d>select persona</d><vt>text</vt><tv>0</tv></p></r></rules>\")%}}",
                    personaInfoByCodeName1.PersonaGUID, personaInfoByCodeName1.PersonaDisplayName),
                identityOption, null);
            personalizationVariant2.VariantDisplayCondition = MacroSecurityProcessor.AddSecurityParameters(
                string.Format(
                    "{{%Rule(\"(Contact.IsInPersona(\\\"{0}\\\"))\", \"<rules><r pos=\\\"0\\\" par=\\\"\\\" op=\\\"and\\\" n=\\\"ContactIsInPersona\\\" ><p n=\\\"_is\\\"><t>is</t><v></v><r>0</r><d>select operation</d><vt>text</vt><tv>0</tv></p><p n=\\\"personaguid\\\"><t>{1}</t><v>{0}</v><r>1</r><d>select persona</d><vt>text</vt><tv>0</tv></p></r></rules>\")%}}",
                    personaInfoByCodeName2.PersonaGUID, personaInfoByCodeName2.PersonaDisplayName),
                identityOption, null);
            personalizationVariant1.VariantEnabled = true;
            personalizationVariant2.VariantEnabled = true;
            ContentPersonalizationVariantInfoProvider.SetContentPersonalizationVariant(personalizationVariant1);
            ContentPersonalizationVariantInfoProvider.SetContentPersonalizationVariant(personalizationVariant2);
        }

        private void GeneratePersonaContactHistory()
        {
            PersonaContactHistoryInfoProvider.GetPersonaContactHistory().ForEachObject(
                PersonaContactHistoryInfoProvider
                    .DeletePersonaContactHistoryInfo);
            GeneratePersonaContactHistoryRecords();
        }

        private void GeneratePersonaContactHistoryRecords()
        {
            var contactsWithoutPersona = 2658;
            var contactsInB2BPersona = 426;
            var contactsInB2CPersona = 1037;
            var personaInfoByCodeName1 = PersonaInfoProvider.GetPersonaInfoByCodeName(PERSONA_CAFE_OWNER);
            var personaInfoByCodeName2 = PersonaInfoProvider.GetPersonaInfoByCodeName(PERSONA_COFEE_GEEK);
            var now = DateTime.Now;
            var dateTime1 = now.AddDays(30.0);
            var dateTime2 = now.AddDays(-60.0);
            var collection = new Collection<PersonaContactHistoryInfo>();
            for (var time = dateTime2; time < dateTime1; time = time.AddDays(1.0))
            {
                IncreaseContactsCount(time, now, ref contactsWithoutPersona, ref contactsInB2BPersona,
                    ref contactsInB2CPersona);
                ChangeCountsIfCampaign1IsRunning(time, now, ref contactsInB2CPersona);
                ChangeCountsIfCampaign2IsSendingMails(time, now, ref contactsWithoutPersona, ref contactsInB2CPersona);
                ChangeCountsWhenPersonaRulesChanged(time, now, ref contactsWithoutPersona, ref contactsInB2BPersona,
                    ref contactsInB2CPersona);
                collection.Add(CreatePersonaContactHistoryInfo(contactsWithoutPersona, time, new int?()));
                collection.Add(CreatePersonaContactHistoryInfo(contactsInB2BPersona, time,
                    personaInfoByCodeName1.PersonaID));
                collection.Add(CreatePersonaContactHistoryInfo(contactsInB2CPersona, time,
                    personaInfoByCodeName2.PersonaID));
            }

            PersonaContactHistoryInfoProvider.BulkInsert(collection);
        }

        private PersonaContactHistoryInfo CreatePersonaContactHistoryInfo(
            int contactsCount,
            DateTime time,
            int? personaId) =>
            new PersonaContactHistoryInfo
            {
                PersonaContactHistoryContacts = contactsCount,
                PersonaContactHistoryDate = time,
                PersonaContactHistoryPersonaID = personaId
            };

        private void IncreaseContactsCount(
            DateTime time,
            DateTime now,
            ref int contactsWithoutPersona,
            ref int contactsInB2BPersona,
            ref int contactsInB2CPersona)
        {
            if (time < now.AddDays(-45.0))
            {
                contactsWithoutPersona += _mRandom.Next(80, 120);
                contactsInB2BPersona += _mRandom.Next(3, 12);
                contactsInB2CPersona += _mRandom.Next(8, 20);
            }
            else
            {
                contactsWithoutPersona += _mRandom.Next(50, 80);
                contactsInB2BPersona += _mRandom.Next(2, 5);
                contactsInB2CPersona += _mRandom.Next(25, 60);
            }
        }

        private void ChangeCountsIfCampaign1IsRunning(
            DateTime time,
            DateTime now,
            ref int contactsInB2CPersona)
        {
            if (!(time > now.AddDays(-14.0)) || !(time < now.AddDays(-8.0)))
            {
                return;
            }

            contactsInB2CPersona += _mRandom.Next(20, 40);
        }

        private void ChangeCountsIfCampaign2IsSendingMails(
            DateTime time,
            DateTime now,
            ref int contactsWithoutPersona,
            ref int contactsInB2CPersona)
        {
            if (!(time > now.AddDays(-14.0)) || !(time < now.AddDays(-11.0)))
            {
                return;
            }

            contactsWithoutPersona += _mRandom.Next(60, 100);
            contactsInB2CPersona += _mRandom.Next(90, 150);
        }

        private void ChangeCountsWhenPersonaRulesChanged(
            DateTime time,
            DateTime now,
            ref int contactsWithoutPersona,
            ref int contactsInB2BPersona,
            ref int contactsInB2CPersona)
        {
            if (!(time.Date > now.Date.AddDays(-45.0)) || !(time.Date <= now.Date.AddDays(-44.0)))
            {
                return;
            }

            contactsWithoutPersona -= 235;
            contactsInB2BPersona -= sbyte.MaxValue;
            contactsInB2CPersona += 382;
        }
    }
}