using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    public class SearchCriteria : CriteriaBase
    {
        public int? LevelID { get; set; }

        public int? Currency { get; set; }

        public int? SiteID { get; set; }

        public int? BetLimitTypeID { get; set; }

        public string Username { get; set; }

        public int UserId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "NAP_BLControl_CustSelect";

        public override bool IsValid()
            => UserId > 0 && PageSize > 0 && PageIndex > 0;
    }
}