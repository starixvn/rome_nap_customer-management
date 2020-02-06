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

        [SpParam(Ignored = true)]
        public bool IsGlobalSetting { get; set; }

        public override string GetSettingKey()
            => Username == null ? "Acc_CustVirtualMultiplier_Sel" : "Acc_CustVirtualMultiplierByUsername_Sel";

        public override Hashtable GetSpParams()
        {
            var result = new Hashtable
            {
                { "sporttype", SportType },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            if (Username == null)
            {
                result.Add("custid", CustId);
            }
            else
            {
                result.Add("username", Username);
            }          
            return result;
        }
    }
}
