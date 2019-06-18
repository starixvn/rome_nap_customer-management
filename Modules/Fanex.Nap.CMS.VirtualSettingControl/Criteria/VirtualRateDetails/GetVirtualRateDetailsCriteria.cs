namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using System.Collections;
    using Fanex.Data.Repository;

    public class GetVirtualRateDetailsCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int CustId { get; set; }

        public int SportType { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => Username == null ? "Acc_CustVirtualMultiplier_Sel" : "Acc_CustVirtualMultiplierByUsername_Sel";

        public override Hashtable GetSpParams()
        {
            var result = new Hashtable();
            result.Add("sporttype", SportType);

            if (Username == null)
            {
                result.Add("custid", CustId);
            }
            else
            {
                result.Add("username", Username);
            }

            result.Add("errcode", 0);
            result.Add("errmsg", string.Empty);
            return result;
        }
    }
}
