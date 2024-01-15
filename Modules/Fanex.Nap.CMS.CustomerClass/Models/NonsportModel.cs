using System;

namespace Fanex.Nap.CMS.CustomerClass.Models
{
    public class NonsportModel
    {
        public NonsportModel(int policyId, string policyTitle, int maxBet, string customerClass, string sites, string products, bool enable, byte isTest, int lastModifiedUser, DateTime lastModifiedTime)
        {
            PolicyId = policyId;
            PolicyTitle = policyTitle;
            CustomerClass = customerClass;
            Sites = sites;
            Products = products;
            MaxBet = maxBet;
            IsEnable = enable;
            IsTest = isTest;
            LastModifiedUser = lastModifiedUser;
            LastModifiedTime = lastModifiedTime;
        }

        public int PolicyId { get; }
        public string PolicyTitle { get; }
        public string CustomerClass { get; }
        public string Sites { get; }
        public string Products { get; }
        public int MaxBet { get; }
        public bool IsEnable { get; }
        public byte IsTest { get; }
        public int LastModifiedUser { get; }
        public DateTime LastModifiedTime { get; }
    }
}
