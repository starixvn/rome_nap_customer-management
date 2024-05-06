namespace Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine
{
    using System.Text;

    public class AddNewRuleViewModel
    {
        public string RuleTitle { get; set; }

        public int PriorityOrder { get; set; }

        // [1]
        public byte? SiteType { get; set; }
        public string Site { get; set; }
        public string SiteNotIn { get; set; }

        // [2]
        public string CustomerClass { get; set; }
        public string CustomerClassNotIn { get; set; }

        // [3]
        public string CustomerClassDetail { get; set; }
        public string CustomerClassDetailNotIn { get; set; }
        public string CCSportType { get; set; }
        public string CCSportTypeNotIn { get; set; }

        // [4]
        public decimal? VRFrom { get; set; }
        public decimal? VRTo { get; set; }

        // [5]
        public string SportType { get; set; }
        public string SportTypeNotIn { get; set; }

        // [6]
        public string BetType { get; set; }
        public string BetTypeNotIn { get; set; }

        // [7]
        public byte? LiveIndicator { get; set; }

        // [8]
        public string LeagueGroup { get; set; }
        public string LeagueGroupNotIn { get; set; }
        public string League { get; set; }
        public string LeagueNotIn { get; set; }

        // [9]
        public string Betfrom { get; set; }

        // [10]
        public decimal? StakeFrom { get; set; }
        public decimal? StakeTo { get; set; }

        // [11]
        public string DayOfWeek { get; set; }
        public string DayOfWeekNotIn { get; set; }

        // [12]
        public string BeginTime { get; set; }
        public string EndTime { get; set; }

        // [13]
        public string CurrencyIDList { get; set; }

        public string CurrencyIDNotIn { get; set; }

        public byte AdjustmentType { get; set; }

        public decimal AdjustmentValue { get; set; }

        public byte AccumulatedBalance { get; set; }

        public byte Active { get; set; }

        public string CriteriaNotApply
        {
            get
            {
                var criteriaRows = new StringBuilder();
                criteriaRows.Append("<Root>");

                if (!string.IsNullOrWhiteSpace(SiteNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"SiteIDList\" Value=\"{0}\"/>", SiteNotIn);
                }

                if (!string.IsNullOrWhiteSpace(CustomerClassNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"CustomerClassList\" Value=\"{0}\"/>", CustomerClassNotIn);
                }

                if (!string.IsNullOrWhiteSpace(CustomerClassDetailNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"CCSporttypeList\" Value=\"{0}\"/>", CCSportTypeNotIn);
                    criteriaRows.AppendFormat("<Row Criteria=\"CustomerClassDetailList\" Value=\"{0}\"/>", CustomerClassDetailNotIn);
                }

                if (!string.IsNullOrWhiteSpace(SportTypeNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"SporttypeList\" Value=\"{0}\"/>", SportTypeNotIn);
                }

                if (!string.IsNullOrWhiteSpace(BetTypeNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"BettypeList\" Value=\"{0}\"/>", BetTypeNotIn);
                }

                if (!string.IsNullOrWhiteSpace(LeagueGroupNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"LeagueGroupList\" Value=\"{0}\"/>", LeagueGroupNotIn);
                }

                if (!string.IsNullOrWhiteSpace(LeagueNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"LeagueList\" Value=\"{0}\"/>", LeagueNotIn);
                }

                if (!string.IsNullOrWhiteSpace(DayOfWeekNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"DateOfWeekList\" Value=\"{0}\"/>", DayOfWeekNotIn);
                }

                if (!string.IsNullOrWhiteSpace(CurrencyIDNotIn))
                {
                    criteriaRows.AppendFormat("<Row Criteria=\"CurrencyIDList\" Value=\"{0}\"/>", CurrencyIDNotIn);
                }

                criteriaRows.Append("</Root>");
                return criteriaRows.ToString();
            }
        }

        public string EnabledColumns { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }
    }
}
