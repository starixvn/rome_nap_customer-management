namespace Fanex.Nap.CMS.Customers.Criteria.ManageRobots2
{
    using System.Collections;
    using Fanex.Data.Repository;

    public class GetRobotsCriteria : CriteriaBase
    {
        public string Username { get; set; } = string.Empty;

        public int Level { get; set; }

        public int Currency { get; set; }

        public string Site { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

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
                { "totalitems", 0 }
            };

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
    }
}
