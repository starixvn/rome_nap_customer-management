namespace Fanex.Nap.CMS.Customers.Models.VirtualRateEngine
{
    using System;

    public class VREngineRule
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public int Priority { get; set; }
            int preMatchPeriod,
            PreMatchPeriod = preMatchPeriod;


        public byte SiteType { get; set; }
        public string SiteIdList { get; set; } = string.Empty;
        public string SiteIdListNotIn { get; set; } = string.Empty;
        public bool EnabledSiteType { get; set; }
        public string SiteTypeName { get; set; }
        public string SiteNameList { get; set; } = string.Empty;
        public string SiteNameListNotIn { get; set; } = string.Empty;

        public string CustomerClassList { get; set; } = string.Empty;
        public string CustomerClassListNotIn { get; set; } = string.Empty;
        public bool EnabledCustomerClass { get; set; }

        public string CustomerClassDetailList { get; set; } = string.Empty;
        public string CustomerClassDetailListNotIn { get; set; } = string.Empty;
        public string SportTypeCCList { get; set; } = string.Empty;
        public string SportTypeCCListNotIn { get; set; } = string.Empty;
        public string SportTypeCCNameList { get; set; } = string.Empty;
        public string SportTypeCCNameListNotIn { get; set; } = string.Empty;
        public bool EnabledCustomerClassDetail { get; set; }

        public decimal VRFrom { get; set; }
        public decimal VRTo { get; set; }
        public bool EnabledCurrentVR { get; set; }

        public byte VRStatus { get; set; }
        public bool EnabledCurrentVRStatus { get; set; }
        public string VRStatusName { get; set; }

        public string SportTypeList { get; set; } = string.Empty;
        public string SportTypeListNotIn { get; set; } = string.Empty;
        public bool EnabledSport { get; set; }
        public string SportTypeNameList { get; set; } = string.Empty;
        public string SportTypeNameListNotIn { get; set; } = string.Empty;

        public string BetTypeList { get; set; } = string.Empty;
        public string BetTypeListNotIn { get; set; } = string.Empty;
        public bool EnabledBetType { get; set; }
        public string BetTypeNameList { get; set; } = string.Empty;
        public string BetTypeNameListNotIn { get; set; } = string.Empty;

        public byte LiveIndicator { get; set; }
        public bool EnabledEvent { get; set; }
        public string LiveIndicatorName { get; set; }

        public string LeagueGroupList { get; set; } = string.Empty;
        public string LeagueGroupListNotIn { get; set; } = string.Empty;
        public string LeagueList { get; set; } = string.Empty;
        public string LeagueListNotIn { get; set; } = string.Empty;
        public bool EnabledLeagueGroup { get; set; }
        public string LeagueGroupNameList { get; set; } = string.Empty;
        public string LeagueGroupNameListNotIn { get; set; } = string.Empty;

        public string Betfrom { get; set; }
        public bool EnabledBetfrom { get; set; }
        public string BetfromName { get; set; }

        public decimal StakeFrom { get; set; }
        public decimal StakeTo { get; set; }
        public bool EnabledStake { get; set; }

        public string DoWList { get; set; } = string.Empty;
        public string DoWListNotIn { get; set; } = string.Empty;
        public bool EnabledTicketDoW { get; set; }
        public string DoWNameList { get; set; }
        public string DoWNameListNotIn { get; set; }

        public TimeSpan BeginTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool EnabledTicketTime { get; set; }

        public string CurrencyIDList { get; set; } = string.Empty;
        public string CurrencyIDListNotIn { get; set; } = string.Empty;
        public string CurrencyNameList { get; set; } = string.Empty;
        public string CurrencyNameListNotIn { get; set; } = string.Empty;
        public bool EnabledTicketCur { get; set; }

        public int PreMatchPeriod { get; }
        public bool EnabledPreMatchPeriod { get; set; }

        public byte AdjustmentType { get; set; }
        public decimal AdjustmentValue { get; set; }
        public bool AccumulatedBalance { get; set; }
        public string AdjustmentCell { get; set; }

        public byte Status { get; set; }

        public string StatusName { get; set; }

        public DateTime CreatedDTS { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string EnabledCriteria { get; set; }
    }
}
