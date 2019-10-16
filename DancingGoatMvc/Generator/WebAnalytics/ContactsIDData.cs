using System.Linq;
using CMS.DataEngine;

namespace DancingGoat.Generator.WebAnalytics
{
    public class ContactsIdData
    {
        private readonly int[] _mContactIDs;
        private readonly int _mContactsCount;
        private int _mCurrectContact;

        public ContactsIdData(string emailPostFix, int contactsCount)
        {
            _mContactIDs = new ObjectQuery("om.contact").Column("ContactID")
                .WhereEndsWith("ContactEmail", emailPostFix).TopN(contactsCount).GetListResult<int>()
                .ToArray();

            _mCurrectContact = 0;
            _mContactsCount = _mContactIDs.Length;
        }

        public int GetNextContactId()
        {
            var mContactId = _mContactIDs[_mCurrectContact];
            _mCurrectContact = (_mCurrectContact + 1) % _mContactsCount;
            return mContactId;
        }
    }
}