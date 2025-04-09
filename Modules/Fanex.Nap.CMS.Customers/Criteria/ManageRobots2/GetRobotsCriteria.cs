namespace Fanex.Nap.CMS.Customers.Criteria.ManageRobots2
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetRobotsCriteria : CriteriaBase
    {
        public string Username { get; set; } = string.Empty;

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? Level { get; set; }

        public int? Currency { get; set; }

        public string Site { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string SortColumnName { get; set; }

        [SpParam(SqlParamName = "ordertype")]
        public string SortDirection { get; set; } = string.Empty;

        public int OrderOption => GetOrderOption();

        [SpParam(Ignored = true)]
        public int ErrCode { get; set; }

        [SpParam(Ignored = true)]
        public string ErrMsg { get; set; } = string.Empty;

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "NAP_Customer_RobotAccountNew_Sel";

        public override bool IsValid()
            => true;

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