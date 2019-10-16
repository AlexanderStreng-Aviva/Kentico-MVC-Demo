using System;
using System.Linq;
using CMS.Activities;
using CMS.Base;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.SiteProvider;
using CMS.WebAnalytics;

namespace DancingGoat.Generator.WebAnalytics
{
    public class OnlineMarketingDataGenerator
    {
        private const int NumberOfAnonymousContacts = 5;
        private const string CeoContactRole = "CEO";
        private const string OwnerContactRole = "Owner";
        private const string BaristaContactRole = "Barista";
        private const string ContactCampaign = "CafeSamplePromotion";
        private const string ContactUsFormCodeName = "ContactUs";
        private const string TryFreeSampleFormCodeName = "TryAFreeSample";
        private const string BusinessCustomerRegistationFormCodeName = "BusinessCustomerRegistration";
        private readonly FormActivityGenerator _mFormActivityGenerator = new FormActivityGenerator();

        private readonly TreeNode _mHomeDocument = DocumentHelper.GetDocuments().All().Culture("en-US").Path("/Home")
            .OnCurrentSite().TopN(1).FirstOrDefault();

        private readonly PagesActivityGenerator _mPagesActivityGenerator = new PagesActivityGenerator();

        private readonly TreeNode _mPartnershipDocument = DocumentHelper.GetDocuments().All().Culture("en-US")
            .Path("/Partnership").OnCurrentSite().TopN(1).FirstOrDefault();

        private readonly SiteInfo _mSiteInfo;

        /// <summary>Constructor.</summary>
        /// <param name="site">Site data will be generated for</param>
        public OnlineMarketingDataGenerator(SiteInfo site)
        {
            _mSiteInfo = site;
        }

        /// <summary>Performs contacts and activities generation.</summary>
        public void Generate()
        {
            GenerateAnonymousContacts();
            GenerateContactsWithContactProfileData();
        }

        private void GenerateAnonymousContacts()
        {
            for (var index = 0; index < NumberOfAnonymousContacts; ++index)
            {
                var dateTime = DateTime.Now;
                dateTime = dateTime.AddDays(-index);
                dateTime = dateTime.AddHours(-index);
                dateTime = dateTime.AddMinutes(-index);
                CreateAnonymousContact(dateTime.AddSeconds(-index));
            }
        }

        private void CreateAnonymousContact(DateTime contactCreated)
        {
            var contactInfo = new ContactInfo
            {
                ContactLastName = "Anonymous - " + contactCreated.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                ContactMonitored = true,
                ContactCreated = contactCreated
            };
            ContactInfoProvider.SetContactInfo(contactInfo);
            GeneratePageVisitActivity(_mHomeDocument, contactInfo);
        }

        private void GenerateContactsWithContactProfileData()
        {
            CreateContactGroup("CoffeeClubMembershipRecipients", "Coffee club membership recipients");
            var account1 = CreateAccount("Air Cafe");
            var account2 = CreateAccount("Jasper Coffee");
            var contactStatus = CreateContactStatus("ProspectiveClient", "Prospective client");
            var contactGroup1 = CreateContactGroup("ProspectiveClients", "Prospective clients");
            var contactGroup2 = CreateContactGroup("ImportedContacts", "Imported contacts");
            var contactRole1 = CreateContactRole(OwnerContactRole);
            var contactRole2 = CreateContactRole(CeoContactRole);
            var contactRole3 = CreateContactRole(BaristaContactRole);
            var userId = UserInfoProvider.GetUserInfo("alex").UserID;
            var monicaKing = GenerateMonicaKing(contactStatus.ContactStatusID, userId);
            var dustinEvans = GenerateDustinEvans(contactStatus.ContactStatusID, userId);
            var toddRay = GenerateToddRay(contactStatus.ContactStatusID, userId);
            GenerateStacyStewart();
            GenerateHaroldLarson();
            AssignCustomerToContact(monicaKing, "Monica");
            AssignCustomerToContact(dustinEvans, "Dustin");
            AddContactToContactGroup(monicaKing, contactGroup1);
            AddContactToContactGroup(dustinEvans, contactGroup1);
            AddContactToContactGroup(toddRay, contactGroup1);
            AddContactToContactGroup(monicaKing, contactGroup2);
            AddContactToContactGroup(dustinEvans, contactGroup2);
            AddContactToContactGroup(toddRay, contactGroup2);
            AssignContactToAccount(account1.AccountID, toddRay, contactRole1);
            AssignContactToAccount(account1.AccountID, monicaKing, contactRole3);
            AssignContactToAccount(account2.AccountID, dustinEvans, contactRole2);
        }

        private void GenerateHaroldLarson()
        {
            var contact = GenerateContact("Harold", "Larson", "Harold.Larson@localhost.local", "(742)-343-5223");
            contact.ContactGender = 1;
            contact.ContactCountryID = CountryInfoProvider.GetCountryInfo("USA").CountryID;
            contact.ContactCity = "Bedford";
            contact.ContactBounces = 5;
            contact.ContactStateID = StateInfoProvider.GetStateInfo("NewHampshire").StateID;
            ContactInfoProvider.SetContactInfo(contact);
            GeneratePageVisitActivity(_mPartnershipDocument, contact);
            CreateFormSubmission(_mPartnershipDocument, TryFreeSampleFormCodeName, contact);
            CreateFormSubmission(_mPartnershipDocument, ContactUsFormCodeName, contact);
        }

        private void GenerateStacyStewart()
        {
            var contact = GenerateContact("Stacy", "Stewart", "Stacy.Stewart@localhost.local", null);
            contact.ContactCountryID = CountryInfoProvider.GetCountryInfo("Germany").CountryID;
            contact.ContactCity = "Berlin";
            contact.ContactCampaign = ContactCampaign;
            contact.ContactNotes = "Contact acquired at CoffeeExpo2015";
            ContactInfoProvider.SetContactInfo(contact);
            GeneratePageVisitActivity(_mHomeDocument, contact);
            GeneratePageVisitActivity(_mPartnershipDocument, contact);
            CreateFormSubmission(_mPartnershipDocument, TryFreeSampleFormCodeName, contact);
        }

        private ContactInfo GenerateToddRay(int contactStatusId, int contactOwneruserId)
        {
            var contact = GenerateContact("Todd", "Ray", "Todd.Ray@localhost.local", "(808)-289-4459");
            contact.ContactBirthday = DateTime.Today.AddYears(-42);
            contact.ContactGender = 1;
            contact.ContactJobTitle = OwnerContactRole;
            contact.ContactStatusID = contactStatusId;
            contact.ContactMobilePhone = "+420123456789";
            contact.ContactCampaign = ContactCampaign;
            contact.ContactOwnerUserID = contactOwneruserId;
            contact.ContactCity = "Brno";
            contact.ContactAddress1 = "Benesova 13";
            contact.ContactZIP = "612 00";
            contact.ContactCompanyName = "Air Cafe";
            contact.ContactCountryID = CountryInfoProvider.GetCountryInfo("CzechRepublic").CountryID;
            contact.ContactNotes = "Should be involved in every communication with Air Cafe.";
            ContactInfoProvider.SetContactInfo(contact);
            GeneratePageVisitActivity(_mPartnershipDocument, contact);
            CreateFormSubmission(_mPartnershipDocument, BusinessCustomerRegistationFormCodeName, contact);
            return contact;
        }

        private ContactInfo GenerateDustinEvans(int contactStatusId, int contactOwneruserId)
        {
            var contact = GenerateContact("Dustin", "Evans", "Dustin.Evans@localhost.local", "(808)-139-4639");
            contact.ContactBirthday = DateTime.Today.AddYears(-40);
            contact.ContactGender = 1;
            contact.ContactJobTitle = CeoContactRole;
            contact.ContactStatusID = contactStatusId;
            contact.ContactMobilePhone = "+420123456789";
            contact.ContactCampaign = ContactCampaign;
            contact.ContactOwnerUserID = contactOwneruserId;
            contact.ContactNotes = "Willing to participate in the partnership program - materials sent";
            contact.ContactCity = "South Yarra";
            contact.ContactAddress1 = "163 Commercial Road";
            contact.ContactZIP = "VIC 3141";
            contact.ContactCompanyName = "Jasper Coffee";
            contact.ContactCountryID = CountryInfoProvider.GetCountryInfo("Australia").CountryID;
            contact.ContactNotes = "Willing to participate in the partnership program - materials sent";
            ContactInfoProvider.SetContactInfo(contact);
            GeneratePageVisitActivity(_mHomeDocument, contact);
            GenerateInternalSearchActivity(_mHomeDocument, contact, "wholesale");
            GeneratePageVisitActivity(_mPartnershipDocument, contact);
            CreateFormSubmission(_mPartnershipDocument, BusinessCustomerRegistationFormCodeName, contact);
            return contact;
        }

        private ContactInfo GenerateMonicaKing(int contactStatusId, int contactOwneruserId)
        {
            var contact = GenerateContact("Monica", "King", "monica.king@localhost.local", "(595)-721-1648");
            contact.ContactBirthday = DateTime.Today.AddYears(-35);
            contact.ContactGender = 2;
            contact.ContactJobTitle = BaristaContactRole;
            contact.ContactStatusID = contactStatusId;
            contact.ContactMobilePhone = "+420123456789";
            contact.ContactCampaign = ContactCampaign;
            contact.ContactOwnerUserID = contactOwneruserId;
            contact.ContactCity = "Brno";
            contact.ContactAddress1 = "New Market 187/5";
            contact.ContactZIP = "602 00";
            contact.ContactCompanyName = "Air Cafe";
            contact.ContactCountryID = CountryInfoProvider.GetCountryInfo("CzechRepublic").CountryID;
            contact.ContactNotes = "Should be involved in every communication with Air Cafe.";
            ContactInfoProvider.SetContactInfo(contact);
            GeneratePageVisitActivity(_mPartnershipDocument, contact);
            CreateFormSubmission(_mPartnershipDocument, TryFreeSampleFormCodeName, contact);
            CreateFormSubmission(_mPartnershipDocument, ContactUsFormCodeName, contact);
            GeneratePurchaseActivity(20.0, contact);
            return contact;
        }

        private void AssignContactToAccount(
            int accountAccountId,
            ContactInfo monicaKing,
            ContactRoleInfo contactRoleInfo)
        {
            AccountContactInfoProvider.SetAccountContactInfo(new AccountContactInfo
            {
                ContactID = monicaKing.ContactID,
                AccountID = accountAccountId,
                ContactRoleID = contactRoleInfo.ContactRoleID
            });
        }

        private ContactRoleInfo CreateContactRole(string contactRoleCodeName)
        {
            var contactRoleInfo = ContactRoleInfoProvider.GetContactRoleInfo(contactRoleCodeName);
            if (contactRoleInfo != null)
            {
                ContactRoleInfoProvider.DeleteContactRoleInfo(contactRoleInfo);
            }

            var roleObj = new ContactRoleInfo();
            roleObj.ContactRoleDescription = contactRoleCodeName;
            roleObj.ContactRoleDisplayName = contactRoleCodeName;
            roleObj.ContactRoleName = contactRoleCodeName;
            ContactRoleInfoProvider.SetContactRoleInfo(roleObj);
            return roleObj;
        }

        private ContactStatusInfo CreateContactStatus(
            string contactStatusCodeName,
            string contactStatusDisplayName)
        {
            var contactStatusInfo = ContactStatusInfoProvider.GetContactStatusInfo(contactStatusCodeName);
            if (contactStatusInfo != null)
            {
                ContactStatusInfoProvider.DeleteContactStatusInfo(contactStatusInfo);
            }

            var statusObj = new ContactStatusInfo();
            statusObj.ContactStatusDescription = contactStatusDisplayName;
            statusObj.ContactStatusDisplayName = contactStatusDisplayName;
            statusObj.ContactStatusName = contactStatusCodeName;
            ContactStatusInfoProvider.SetContactStatusInfo(statusObj);
            return statusObj;
        }

        private ContactInfo GenerateContact(
            string firstName,
            string lastName,
            string email,
            string businessPhone)
        {
            var contactObj = new ContactInfo();
            contactObj.ContactFirstName = firstName;
            contactObj.ContactLastName = lastName;
            contactObj.ContactEmail = email;
            contactObj.ContactBusinessPhone = businessPhone;
            contactObj.ContactMonitored = true;
            ContactInfoProvider.SetContactInfo(contactObj);
            return contactObj;
        }

        private void GenerateInternalSearchActivity(
            TreeNode homeDocument,
            ContactInfo contact,
            string searchKeyword)
        {
            _mPagesActivityGenerator.GenerateInternalSearchActivity(searchKeyword, homeDocument, contact.ContactID,
                _mSiteInfo.SiteID);
        }

        private void CreateFormSubmission(ITreeNode document, string formName, ContactInfo contact)
        {
            var dataClassInfo =
                DataClassInfoProviderBase<DataClassInfoProvider>.GetDataClassInfo(BizFormInfoProvider
                    .GetBizFormInfo(formName, _mSiteInfo.SiteID).FormClassID);
            var formItem = new BizFormItem(dataClassInfo.ClassName);
            _mFormActivityGenerator.GenerateFormSubmitActivity(formItem, document, contact.ContactID,
                _mSiteInfo.SiteID);
            CopyDataFromContactToForm(contact, dataClassInfo, formItem);
            SetFormSpecificData(formName, contact, formItem);
            formItem.Insert();
        }

        private static void CopyDataFromContactToForm(
            BaseInfo contact,
            DataClassInfo classInfo,
            ISearchable formItem)
        {
            foreach (var field in new FormInfo(classInfo.ClassContactMapping).GetFields(true, true))
            {
                formItem.SetValue(field.MappedToField, contact.GetStringValue(field.Name, string.Empty));
            }
        }

        private void SetFormSpecificData(string formName, ContactInfo contact, BizFormItem formItem)
        {
            if (formName == TryFreeSampleFormCodeName)
            {
                formItem.SetValue("Country",
                    CountryInfoProvider.GetCountryInfo(contact.ContactCountryID).CountryThreeLetterCode);
                var stateInfo = StateInfoProvider.GetStateInfo(contact.ContactStateID);
                var str = stateInfo != null ? stateInfo.StateDisplayName : string.Empty;
                formItem.SetValue("State", str);
            }

            if (formName == ContactUsFormCodeName)
            {
                formItem.SetValue("UserMessage", "Message");
            }

            if (formName != BusinessCustomerRegistationFormCodeName)
            {
                return;
            }

            formItem.SetValue("BecomePartner", "Becoming a partner café");
        }

        private void GeneratePageVisitActivity(TreeNode document, ContactInfo contact)
        {
            _mPagesActivityGenerator.GeneratePageVisitActivity(document, contact.ContactID, _mSiteInfo.SiteID);
        }

        private void GeneratePurchaseActivity(double spent, ContactInfo contact)
        {
            var activityTitleBuilder = new ActivityTitleBuilder();
            ActivityInfoProvider.SetActivityInfo(new ActivityInfo
            {
                ActivityTitle = activityTitleBuilder.CreateTitle("purchase", "$" + spent),
                ActivityValue = spent.ToString(CultureHelper.EnglishCulture),
                //ActivityURL = "/",
                ActivityItemID = 0,
                ActivityType = "purchase",
                ActivitySiteID = _mSiteInfo.SiteID,
                ActivityContactID = contact.ContactID
            });
        }

        private void AddContactToContactGroup(ContactInfo contact, ContactGroupInfo contactGroup)
        {
            ContactGroupMemberInfoProvider.SetContactGroupMemberInfo(new ContactGroupMemberInfo
            {
                ContactGroupMemberContactGroupID = contactGroup.ContactGroupID,
                ContactGroupMemberType = ContactGroupMemberTypeEnum.Contact,
                ContactGroupMemberRelatedID = contact.ContactID
            });
        }

        private ContactGroupInfo CreateContactGroup(
            string contactGroupCodeName,
            string contactGroupName)
        {
            var contactGroupInfo = ContactGroupInfoProvider.GetContactGroupInfo(contactGroupCodeName);
            if (contactGroupInfo != null)
            {
                ContactGroupInfoProvider.DeleteContactGroupInfo(contactGroupInfo);
            }

            var groupObj = new ContactGroupInfo();
            groupObj.ContactGroupDisplayName = contactGroupName;
            groupObj.ContactGroupName = contactGroupCodeName;
            groupObj.ContactGroupEnabled = true;
            ContactGroupInfoProvider.SetContactGroupInfo(groupObj);
            return groupObj;
        }

        private AccountInfo CreateAccount(string accountName)
        {
            var accountObj1 = AccountInfoProvider.GetAccounts()
                .FirstOrDefault(
                    acc => acc.AccountName == accountName);
            if (accountObj1 != null)
            {
                AccountInfoProvider.DeleteAccountInfo(accountObj1);
            }

            var accountObj2 = new AccountInfo();
            accountObj2.AccountName = accountName;
            AccountInfoProvider.SetAccountInfo(accountObj2);
            return accountObj2;
        }

        private void AssignCustomerToContact(ContactInfo contact, string customerFirstName)
        {
            var scalarResult = new ObjectQuery("ecommerce.customer").WhereEquals("CustomerFirstName", customerFirstName)
                .Column("CustomerID").GetScalarResult<int>();
            if (scalarResult == 0)
            {
                return;
            }

            ContactMembershipInfoProvider.SetMembershipInfo(new ContactMembershipInfo
            {
                ContactID = contact.ContactID,
                MemberType = MemberTypeEnum.EcommerceCustomer,
                RelatedID = scalarResult
            });
        }
    }
}