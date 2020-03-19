namespace Fanex.Nap.CMS.Customers.Criteria.ManageRobots2
{
    using System;
    using System.Collections;
    using Fanex.Data.Repository;

    public class GetRobotsCriteria : CriteriaBase
    {
        public string Username { get; set; } = string.Empty;

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int Level { get; set; }

        public int Currency { get; set; }

        public string Site { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string SortColumnName { get; set; }

        public string SortDirection { get; set; } = string.Empty;

        [SpParam(Ignored = true)]
        public int ErrCode { get; set; }

        [SpParam(Ignored = true)]
        public string ErrMsg { get; set; } = string.Empty;

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "NAP_Customer_RobotAccountNew_Sel";

        public override Hashtable GetSpParams()
        {
            var result = new Hashtable
            {
                { "username", Username },
                { "site", Site },
                { "pagesize", PageSize },
                { "pageindex", PageIndex },
                { "fromDate", FromDate },
                { "toDate", ToDate?.AddDays(1).AddTicks(-1) },
                { "totalitems", 0 }
            };

            result.Add("ordertype", SortDirection);
            result.Add("orderoption", GetOrderOption());

            if (Level != 0)
            {
                result.Add("level", Level);
            }

            if (Currency != 0)
            {
                result.Add("currency", Currency);
            }

            return result;
        }

        private int GetOrderOption()
        {
            switch (SortColumnName)
            {
                case "ColCurrency":
                    return 1;

                case "ColSite":
                    return 2;

                case "ColCountry":
                    return 3;

                case "ColRegion":
                    return 4;

                case "ColLastChangeTime":
                    return 5;

                case "ColCounter":
                    return 6;

                default:
                    return 0;
            }
        }
    }
}