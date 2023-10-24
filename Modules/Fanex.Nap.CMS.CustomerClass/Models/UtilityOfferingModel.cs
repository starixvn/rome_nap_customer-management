namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System;

    public class UtilityOfferingModel
    {
        public UtilityOfferingModel(int policyID, string policyTitle, string utilityOffering, string customerClass, string currencies, string sites, byte isTest, int lastModifiedUser, DateTime lastModifiedTime)
        {
            PolicyId = policyID;
            PolicyTitle = policyTitle;
            UtilityOffering = utilityOffering;
            CustomerClass = customerClass;
            Currencies = currencies;
            Sites = sites;
            IsTest = isTest;
            LastModifiedUser = lastModifiedUser;
            LastModifiedTime = lastModifiedTime;
        }

        public int PolicyId { get; }
        public string PolicyTitle { get; }
        public string CustomerClass { get; }
        public string Currencies { get; }
        public string Sites { get; }
        public string UtilityOffering { get; }
        public byte IsTest { get; }
        public int LastModifiedUser { get; }
        public DateTime LastModifiedTime { get; }
    }
}
