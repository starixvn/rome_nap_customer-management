namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using System.Collections;
    using Fanex.Data.Repository;

    public class GetWinLimitMonitorCriteria : CriteriaBase
    {
        public int Currency { get; set; }

        public int Game { get; set; }

        public int Status { get; set; }

        public string Username { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRow { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimitControl_WinLimitMonitorSel";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            {
                { "username", Username },
                { "exchangeid", Currency },
                { "isblocked", Status == 1 },
                { "productid", Game },
                { "pageindex", PageIndex },
                { "pagesize", PageSize },
                { "totalrow", TotalRow },
            };
        }
    }
}
