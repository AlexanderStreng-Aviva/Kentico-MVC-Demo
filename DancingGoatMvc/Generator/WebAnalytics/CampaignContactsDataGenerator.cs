using System.Linq;
using CMS.ContactManagement;

namespace DancingGoat.Generator.WebAnalytics
{
    public class CampaignContactsDataGenerator
    {
        private const int NumberOfGeneratedContacts = 531;
        private readonly string _contactFirstNamePrefix;
        private readonly string _contactLastNamePrefix;

        /// <summary>Constructor.</summary>
        /// <param name="contactFirstNamePrefix">First name prefix of contacts generated for sample campaigns.</param>
        /// <param name="contactLastNamePrefix">Last name prefix of contacts generated for sample campaigns.</param>
        public CampaignContactsDataGenerator(
            string contactFirstNamePrefix,
            string contactLastNamePrefix)
        {
            _contactFirstNamePrefix = contactFirstNamePrefix;
            _contactLastNamePrefix = contactLastNamePrefix;
        }

        /// <summary>Performs campaign contacts sample data generating.</summary>
        public void Generate()
        {
            DeleteOldContacts();
            GenerateContacts();
        }

        private void DeleteOldContacts()
        {
            ContactInfoProvider.GetContacts().WhereStartsWith("ContactFirstName", _contactFirstNamePrefix).ToList()
                .ForEach(ContactInfoProvider.DeleteContactInfo);
        }

        private void GenerateContacts()
        {
            for (var index = 0; index < NumberOfGeneratedContacts; ++index)
            {
                ContactInfoProvider.SetContactInfo(new ContactInfo
                {
                    ContactFirstName = _contactFirstNamePrefix + index,
                    ContactLastName = _contactLastNamePrefix + index,
                    ContactEmail = string.Format("{0}{1}@localhost.local", _contactFirstNamePrefix, index)
                });
            }
        }
    }
}