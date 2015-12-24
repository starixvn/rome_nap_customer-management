namespace Fanex.Nap.CMS.FPSClient.Criteria
{
    using System.Collections;
    using Fanex.Data.Repository;

    public class GetCustomerTypeAndSiteCriteria : CriteriaBase
    {
        public int CustomerId { get; set; }

        public bool IsDeposit { get; set; }

        public string Site { get; set; }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            {
                { "CustId", CustomerId },
                { "IsDeposit", IsDeposit },
                { "SiteName", string.Empty }
            };
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_SubscriberSel";
        }
    }
}
