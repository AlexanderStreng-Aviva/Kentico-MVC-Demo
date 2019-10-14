using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Base;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.SiteProvider;

namespace DancingGoat.Generator.WebAnalytics
{
    public class NewslettersDataGenerator
    {
        public const string NEWSLETTER_COLOMBIA_COFFEE_PROMOTION = "ColombiaCoffeePromotion";
        public const string NEWSLETTER_COLOMBIA_COFFEE_PROMOTION_SAMPLE = "ColombiaCoffeeSamplePromotion";
        public const string NEWSLETTER_COFFEE_CLUB_MEMBERSHIP = "CoffeeClubMembership";
        private const string EmailCampaignContactGroup = "CoffeeClubMembershipRecipients";
        private readonly NewsletterActivityGenerator _mNewsletterActivityGenerator = new NewsletterActivityGenerator();
        private readonly SiteInfo _mSite;

        private readonly string[] _mSubscriberNames = new string[100]
        {
            "Deneen Fernald",
            "Antonio Buker",
            "Marlon Loos",
            "Nolan Steckler",
            "Johnetta Tall",
            "Florence Ramsdell",
            "Modesto Speaker",
            "Alissa Ferguson",
            "Calvin Hollier",
            "Diamond Paik",
            "Mardell Dohrmann",
            "Dinorah Clower",
            "Andrea Humbert",
            "Tyrell Galvan",
            "Yong Inskeep",
            "Tom Goldschmidt",
            "Kimbery Rincon",
            "Genaro Kneeland",
            "Roselyn Mulvey",
            "Nancee Jacobson",
            "Jaime Link",
            "Fonda Belnap",
            "Muoi Ishmael",
            "Pearlene Minjarez",
            "Eustolia Studstill",
            "Marilynn Manos",
            "Pamila Turnbow",
            "Lieselotte Satcher",
            "Sharron Mellon",
            "Bennett Heatherington",
            "Spring Hessel",
            "Lashay Blazier",
            "Veronika Lecuyer",
            "Mark Spitz",
            "Peggy Olson",
            "Tyron Bednarczyk",
            "Terese Betty",
            "Bibi Kling",
            "Bruno Spier",
            "Cristen Bussey",
            "Daine Pridemore",
            "Gerald Turpen",
            "Lela Briese",
            "Sharda Bonnie",
            "Omar Martin",
            "Marlyn Pettie",
            "Shiela Cleland",
            "Marica Granada",
            "Garland Reagan",
            "Mora Gillmore",
            "Mariana Rossow",
            "Betty Pollan",
            "Analisa Costilla",
            "Evelyn Mendez",
            "April Rubino",
            "Zachariah Roberson",
            "Sheilah Steinhauser",
            "Araceli Vallance",
            "Lashawna Weise",
            "Charline Durante",
            "Melania Nightingale",
            "Ema Stiltner",
            "Lynelle Threet",
            "Dorcas Cully",
            "Gregg Carranco",
            "Karla Heiner",
            "Judson Siegmund",
            "Alyson Oday",
            "Winston Laxton",
            "Jarod Turrentine",
            "Israel Shanklin",
            "Miquel Jorstad",
            "Brianne Darrow",
            "Tamara Rulison",
            "Elliot Rameriz",
            "Gearldine Nova",
            "Debi Fritts",
            "Leota Cape",
            "Tyler Saleem",
            "Starr Hyden",
            "Loreen Spigner",
            "Raisa Germain",
            "Grace Vigue",
            "Maryann Munsch",
            "Jason Chon",
            "Gisele Mcquillen",
            "Juliane Comeaux",
            "Willette Dodrill",
            "Sherril Weymouth",
            "Ashleigh Dearman",
            "Bret Bourne",
            "Brittney Cron",
            "Lashell Hampson",
            "Barbie Dinwiddie",
            "Ricki Wiener",
            "Bess Pedretti",
            "Lisha Raley",
            "Edgar Schuetz",
            "Jettie Boots",
            "Jefferson Hinkle"
        };

        private readonly Random _rand = new Random(DateTime.Now.Millisecond);

        public NewslettersDataGenerator(SiteInfo site)
        {
            _mSite = site;
        }

        public void Generate()
        {
            GenerateSubscribersForExistingContacts();
            GenerateNewsletterSubscribers();
            GenerateEmailCampaignSubscribers();
        }

        private void GenerateSubscribersForExistingContacts()
        {
            SubscribeContactWithEmailToNewsletter("monica.king@localhost.local", "DancingGoatNewsletter");
            SubscribeContactWithEmailToNewsletter("monica.king@localhost.local", "Coffee101");
            SubscribeContactWithEmailToNewsletter("Dustin.Evans@localhost.local", "DancingGoatNewsletter");
            SubscribeContactWithEmailToNewsletter("Dustin.Evans@localhost.local", "Coffee101");
            SubscribeContactWithEmailToNewsletter("Todd.Ray@localhost.local", "Coffee101");
            SubscribeContactWithEmailToNewsletter("Stacy.Stewart@localhost.local", "Coffee101");
            SubscribeContactWithEmailToNewsletter("Harold.Larson@localhost.local", "DancingGoatNewsletter");
        }

        private void SubscribeContactWithEmailToNewsletter(
            string contactEmail,
            string newsletterCodeName)
        {
            var contactInfo = ContactInfoProvider.GetContactInfo(contactEmail);
            var newsletterInfo =
                NewsletterInfoProvider.GetNewsletterInfo(newsletterCodeName, SiteContext.CurrentSiteID);
            var fullName = string.Format("{0} {1}", contactInfo.ContactFirstName, contactInfo.ContactLastName);
            var subscriber = CreateSubscriber(contactInfo.ContactEmail, contactInfo.ContactFirstName,
                contactInfo.ContactLastName, fullName, contactInfo);
            AssignSubscriberToNewsletter(newsletterInfo.NewsletterID, subscriber);
            _mNewsletterActivityGenerator.GenerateNewsletterSubscribeActivity(newsletterInfo, subscriber.SubscriberID,
                contactInfo.ContactID, _mSite.SiteID);
        }

        private void GenerateNewsletterSubscribers()
        {
            var newsletterInfo = NewsletterInfoProvider.GetNewsletterInfo("Coffee101", _mSite.SiteID);
            if (newsletterInfo == null)
                return;
            var subscribers = GenerateSubscribers();
            AssignAllSubscribersToNewsletter(subscribers, newsletterInfo.NewsletterID);
            AdjustExistingIssues(subscribers);
        }

        private IList<SubscriberInfo> GenerateSubscribers()
        {
            var subscriberInfoList = new List<SubscriberInfo>();
            for (var index = 0; index < _mSubscriberNames.Count(); ++index)
            {
                var fromFullName = SubscriberData.CreateFromFullName(_mSubscriberNames[index]);
                var contact = CreateContact(fromFullName.FirstName, fromFullName.LastName, fromFullName.Email);
                var subscriber = CreateSubscriber(fromFullName.Email, fromFullName.FirstName, fromFullName.LastName,
                    _mSubscriberNames[index], contact);
                subscriberInfoList.Add(subscriber);
            }

            return subscriberInfoList;
        }

        private SubscriberInfo CreateSubscriber(
            string email,
            string firstName,
            string lastName,
            string fullName,
            ContactInfo contact)
        {
            var subscriberByEmail = SubscriberInfoProvider.GetSubscriberByEmail(email, _mSite.SiteID);
            if (subscriberByEmail != null)
                return subscriberByEmail;
            var subscriber = new SubscriberInfo
            {
                SubscriberEmail = email,
                SubscriberFirstName = firstName,
                SubscriberLastName = lastName,
                SubscriberSiteID = _mSite.SiteID,
                SubscriberFullName = fullName,
                SubscriberRelatedID = contact.ContactID,
                SubscriberType = "om.contact"
            };
            SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            return subscriber;
        }

        private ContactInfo CreateContact(
            string firstName,
            string lastName,
            string subscriberEmail)
        {
            var contactObj = new ContactInfo();
            contactObj.ContactFirstName = firstName;
            contactObj.ContactLastName = lastName;
            contactObj.ContactEmail = subscriberEmail;
            ContactInfoProvider.SetContactInfo(contactObj);
            return contactObj;
        }

        private void AssignAllSubscribersToNewsletter(
            IEnumerable<SubscriberInfo> subscribers,
            int newsletterId)
        {
            foreach (var subscriber in subscribers)
                AssignSubscriberToNewsletter(newsletterId, subscriber);
        }

        private void AssignSubscriberToNewsletter(int newsletterId, SubscriberInfo subscriber)
        {
            SubscriberNewsletterInfoProvider.SetSubscriberNewsletterInfo(new SubscriberNewsletterInfo
            {
                SubscriberID = subscriber.SubscriberID,
                NewsletterID = newsletterId,
                SubscriptionApproved = true,
                SubscribedWhen = DateTime.Now
            });
        }

        private void AdjustExistingIssues(IList<SubscriberInfo> subscribers)
        {
            var issue1 = IssueInfoProvider.GetIssues().OnSite(_mSite.SiteID)
                .WhereEquals("IssueSubject", "Coffee 101 - Lesson 1").FirstOrDefault();
            var issue2 = IssueInfoProvider.GetIssues().OnSite(_mSite.SiteID)
                .WhereEquals("IssueSubject", "Coffee 101 - Lesson 2").FirstOrDefault();
            var issue3 = IssueInfoProvider.GetIssues().OnSite(_mSite.SiteID)
                .WhereEquals("IssueSubject", "Get a free Colombia coffee sample today").FirstOrDefault();
            if (issue1 == null || issue2 == null || issue3 == null)
                return;
            SettingsKeyInfoProvider.SetValue("CMSMonitorBouncedEmails", _mSite.SiteName, true);
            issue1.IssueSentEmails = 98;
            issue1.IssueBounces = 2;
            issue1.IssueOpenedEmails = 26;
            issue1.IssueUnsubscribed = 5;
            IssueInfoProvider.SetIssueInfo(issue1);
            issue2.IssueSentEmails = 98;
            issue2.IssueBounces = 0;
            issue2.IssueOpenedEmails = 14;
            issue2.IssueUnsubscribed = 3;
            IssueInfoProvider.SetIssueInfo(issue2);
            issue3.IssueSentEmails = 98;
            issue3.IssueBounces = 0;
            issue3.IssueOpenedEmails = 42;
            issue3.IssueUnsubscribed = 3;
            IssueInfoProvider.SetIssueInfo(issue3);
            var list = subscribers.Select(s => s.SubscriberEmail).ToList();
            var linkTarget1 = string.Format("http://{0}{1}/Store/Coffee/Ethiopia-Yirgacheffe",
                RequestContext.CurrentDomain, SystemContext.ApplicationPath);
            var linkTarget2 = string.Format("http://{0}{1}/Campaign-assets/Cafe-promotion/Colombia",
                RequestContext.CurrentDomain, SystemContext.ApplicationPath);
            GenerateClickedLinksToIssue(issue1.IssueID, linkTarget1, 17, 12, list);
            GenerateClickedLinksToIssue(issue2.IssueID, linkTarget1, 5, 2, list);
            GenerateClickedLinksToIssue(issue3.IssueID, linkTarget2, 35, 34, list);
            GenerateOpenedEmailToIssue(issue1.IssueID, 26, list);
            GenerateOpenedEmailToIssue(issue2.IssueID, 14, list);
            GenerateOpenedEmailToIssue(issue3.IssueID, 42, list);
            GenerateUnsubscriptionsFromIssue(issue1.IssueID, issue1.IssueNewsletterID, 5, subscribers);
            GenerateUnsubscriptionsFromIssue(issue2.IssueID, issue2.IssueNewsletterID, 3, subscribers);
        }

        private void GenerateClickedLinksToIssue(
            int issueId,
            string linkTarget,
            int totalClicks,
            int uniqueClicks,
            IList<string> subscribersEmails)
        {
            var linkObj = new LinkInfo
            {
                LinkIssueID = issueId,
                LinkTarget = linkTarget,
                LinkDescription = "Try Ethiopian Coffee"
            };
            LinkInfoProvider.SetLinkInfo(linkObj);
            for (var index1 = 0; index1 < totalClicks; ++index1)
            {
                var index2 = index1 <= totalClicks - uniqueClicks ? 0 : index1;
                ClickedLinkInfoProvider.SetClickedLinkInfo(new ClickedLinkInfo
                {
                    ClickedLinkEmail = subscribersEmails[index2],
                    ClickedLinkTime = GetRandomDate(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(-1.0)),
                    ClickedLinkNewsletterLinkID = linkObj.LinkID
                });
            }
        }

        private void GenerateOpenedEmailToIssue(
            int issueId,
            int uniqueOpens,
            IList<string> subscribersEmails)
        {
            for (var index = 0; index < uniqueOpens; ++index)
            {
                var infoObj = new OpenedEmailInfo();
                infoObj.OpenedEmailEmail = subscribersEmails[index];
                infoObj.OpenedEmailIssueID = issueId;
                var now = DateTime.Now;
                var from = now.AddMonths(-1);
                now = DateTime.Now;
                var to = now.AddDays(-1.0);
                infoObj.OpenedEmailTime = GetRandomDate(from, to);
                OpenedEmailInfoProvider.SetOpenedEmailInfo(infoObj);
            }
        }

        private void GenerateUnsubscriptionsFromIssue(
            int issueId,
            int newsletterId,
            int unsubscribed,
            IList<SubscriberInfo> subscribers)
        {
            for (var index = 0; index < unsubscribed; ++index)
                CreateUnsubscription(issueId, newsletterId, subscribers[index].SubscriberEmail);
        }

        private void CreateUnsubscription(int issueId, int newsletterId, string unsubscriptionEmail)
        {
            new UnsubscriptionInfo
            {
                UnsubscriptionEmail = unsubscriptionEmail,
                UnsubscriptionNewsletterID = newsletterId,
                UnsubscriptionFromIssueID = issueId
            }.Insert();
        }

        private DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var timeSpan = new TimeSpan((long) (_rand.NextDouble() * (to - from).Ticks));
            return from + timeSpan;
        }

        private void GenerateEmailCampaignSubscribers()
        {
            var contactGroupInfo = ContactGroupInfoProvider.GetContactGroupInfo("CoffeeClubMembershipRecipients");
            var newsletter = NewsletterInfoProvider.GetNewsletterInfo("CoffeeClubMembership", _mSite.SiteID);
            SubscribeContactGroupToIssue(
                IssueInfoProvider.GetIssues().First(issue => issue.IssueNewsletterID == newsletter.NewsletterID),
                contactGroupInfo);
            AddContactsToSubscribedContactGroup(contactGroupInfo);
        }

        private void SubscribeContactGroupToIssue(
            IssueInfo campaignIssue,
            ContactGroupInfo contactGroup)
        {
            IssueContactGroupInfoProvider.SetIssueContactGroupInfo(new IssueContactGroupInfo
            {
                IssueID = campaignIssue.IssueID,
                ContactGroupID = contactGroup.ContactGroupID
            });
        }

        private void AddContactsToSubscribedContactGroup(ContactGroupInfo contactGroup)
        {
            for (var index = 0; index < _mSubscriberNames.Length; ++index)
                if (index % 2 == 0)
                {
                    var fromFullName = SubscriberData.CreateFromFullName(_mSubscriberNames[index]);
                    AddContactToContactGroup(ContactInfoProvider.GetContactInfo(fromFullName.Email), contactGroup);
                    if (index % 10 == 0)
                        CreateGlobalUnsubscription(fromFullName.Email);
                }
        }

        private void CreateGlobalUnsubscription(string unsubscriptionEmail)
        {
            new UnsubscriptionInfo
            {
                UnsubscriptionEmail = unsubscriptionEmail
            }.Insert();
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

        private class SubscriberData
        {
            public string FirstName { get; private set; }

            public string LastName { get; private set; }

            public string Email { get; private set; }

            public static SubscriberData CreateFromFullName(
                string fullName)
            {
                var strArray = fullName.Trim().Split(' ');
                var str1 = strArray[0];
                var str2 = strArray[1];
                var str3 = string.Format("{0}@{1}.local", str1.ToLowerInvariant(), str2.ToLowerInvariant());
                return new SubscriberData
                {
                    FirstName = str1,
                    LastName = str2,
                    Email = str3
                };
            }
        }
    }
}