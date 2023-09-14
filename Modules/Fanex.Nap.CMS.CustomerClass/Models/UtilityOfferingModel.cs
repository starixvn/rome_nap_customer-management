namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System;

    public class UtilityOfferingModel
    {
        public UtilityOfferingModel(int policyId, string policyTitle, string customerClass, string currencies, string sites, string utilityOffering, int lastModifiedUser, DateTime lastModifiedTime)
        {
            PolicyId = policyId;
            PolicyTitle = policyTitle;
            CustomerClass = customerClass;
            Currencies = currencies;
            Sites = sites;
            UtilityOffering = utilityOffering;
            LastModifiedUser = lastModifiedUser;
            LastModifiedTime = lastModifiedTime;
        }

        public int PolicyId { get; }
        public string PolicyTitle { get; }
        public string CustomerClass { get; }
        public string Currencies { get; }
        public string Sites { get; }
        public string UtilityOffering { get; }
        public int LastModifiedUser { get; }
        public DateTime LastModifiedTime { get; }
    }
}
