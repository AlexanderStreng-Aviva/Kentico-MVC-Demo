using System;
using CMS.ContactManagement;
using CMS.Newsletters;
using CMS.SiteProvider;

namespace DancingGoat.Generator.WebAnalytics
{
    public class ContactGroupSubscribersDataGenerator
    {
        private const string ContactGroupAllContactsWithEmail = "AllContactsWithEmail";
        private const string ContactGroupAllChicagoContactsWithEmail = "AllChicagoContactsWithEmail";
        private readonly SiteInfo _mSite;

        /// <summary>Constructor.</summary>
        /// <param name="site">Site the newsletters data will be generated for</param>
        public ContactGroupSubscribersDataGenerator(SiteInfo site)
        {
            _mSite = site;
        }

        /// <summary>Performs contact group sample data generation.</summary>
        public void Generate()
        {
            AddContactGroupSubscribers();
        }

        private void AddContactGroupSubscribers()
        {
            AddContactGroupSubscriber("AllContactsWithEmail", "ColombiaCoffeePromotion", _mSite.SiteName);
            AddContactGroupSubscriber("AllChicagoContactsWithEmail", "ColombiaCoffeeSamplePromotion", _mSite.SiteName);
        }

        private void AddContactGroupSubscriber(
            string contactGroupName,
            string newsletterName,
            string siteName)
        {
            var contactGroupInfo = ContactGroupInfoProvider.GetContactGroupInfo(contactGroupName);
            if (contactGroupInfo == null)
                return;
            var siteInfo = SiteInfoProvider.GetSiteInfo(siteName);
            var newsletterInfo = NewsletterInfoProvider.GetNewsletterInfo(newsletterName, siteInfo.SiteID);
            if (newsletterInfo == null ||
                SubscriberInfoProvider.GetSubscriberInfo("om.contactgroup", contactGroupInfo.ContactGroupID,
                    siteInfo.SiteID) != null)
                return;
            var subscriber = new SubscriberInfo();
            subscriber.SubscriberType = "om.contactgroup";
            subscriber.SubscriberRelatedID = contactGroupInfo.ContactGroupID;
            subscriber.SubscriberSiteID = siteInfo.SiteID;
            subscriber.SubscriberFirstName = contactGroupInfo.ContactGroupDisplayName;
            subscriber.SubscriberFullName =
                string.Format("Contact group '{0}'", contactGroupInfo.ContactGroupDisplayName);
            SubscriberInfoProvider.SetSubscriberInfo(subscriber);
            SubscriberNewsletterInfoProvider.AddSubscriberToNewsletter(subscriber.SubscriberID,
                newsletterInfo.NewsletterID, DateTime.Now, true);
            SubscriberNewsletterInfoProvider.AddSubscriberToNewsletter(subscriber.SubscriberID,
                newsletterInfo.NewsletterID, DateTime.Now, true);
        }
    }
}