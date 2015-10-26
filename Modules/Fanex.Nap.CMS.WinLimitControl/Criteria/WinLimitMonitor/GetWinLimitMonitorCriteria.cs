namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using System;
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

        public string SortColumnName { get; set; }

        public string SortDirection { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimitControl_WinLimitMonitorSel";
        }

        public override Hashtable GetSpParams()
        {
            var inputParams = new Hashtable 
            {
                { "username", Username },
                { "productid", Game },
                { "exchangeid", Currency },
                { "isblocked", Status == 1 },
                { "pageindex", PageIndex },
                { "pagesize", PageSize },
                { "totalrow", TotalRow },
                { "sortorder", 1 }
            };

            if (SortColumnName.Equals("colUsername", StringComparison.OrdinalIgnoreCase))
            {
                if (SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    inputParams["sortorder"] = 2;
                }
            }

            return inputParams;
        }
    }
}
