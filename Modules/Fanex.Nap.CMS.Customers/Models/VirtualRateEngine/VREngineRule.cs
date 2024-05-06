namespace Fanex.Nap.CMS.Customers.Models.VirtualRateEngine
{
    using System;

    public class VREngineRule
    {
        public VREngineRule(
            int ruleId,
            string ruleName,
            int priority,
            byte siteType,
            string siteIdList,
            string siteIdListNotIn,
            string customerClassList,
            string customerClassListNotIn,
            string customerClassDetailList,
            string customerClassDetailListNotIn,
            string sportTypeCCList,
            string sportTypeCCListNotIn,
            decimal vrFrom,
            decimal vrto,
            byte vrStatus,
            string sportTypeList,
            string sportTypeListNotIn,
            string betTypeList,
            string betTypeListNotIn,
            byte liveIndicator,
            string leagueGroupList,
            string leagueGroupListNotIn,
            string leagueList,
            string leagueListNotIn,
            string betfrom,
            decimal stakeFrom,
            decimal stakeTo,
            string dateOfWeekList,
            string dateOfWeekListNotIn,
            string beginTime,
            string endTime,
            string currencyIDList,
            string currencyIDListNotIn,
            byte adjustmentType,
            decimal adjustmentValue,
            byte accumulatedBalance,
            byte status,
            DateTime createdDTS,
            string createdBy,
            string enabledCriteria)
        {
            RuleId = ruleId;
            RuleName = ruleName;
            Priority = priority;

            SiteType = siteType;
            SiteIdList = siteIdList;
            SiteIdListNotIn = siteIdListNotIn;

            CustomerClassList = customerClassList;
            CustomerClassListNotIn = customerClassListNotIn;

            CustomerClassDetailList = customerClassDetailList;
            CustomerClassDetailListNotIn = customerClassDetailListNotIn;
            SportTypeCCList = sportTypeCCList;
            SportTypeCCListNotIn = sportTypeCCListNotIn;

            VRFrom = vrFrom;
            VRTo = vrto;

            VRStatus = vrStatus;

            SportTypeList = sportTypeList;
            SportTypeListNotIn = sportTypeListNotIn;

            BetTypeList = betTypeList;
            BetTypeListNotIn = betTypeListNotIn;

            LiveIndicator = liveIndicator;

            LeagueGroupList = leagueGroupList;
            LeagueGroupListNotIn = leagueGroupListNotIn;
            LeagueList = leagueList;
            LeagueListNotIn = leagueListNotIn;

            Betfrom = betfrom;

            StakeFrom = stakeFrom;
            StakeTo = stakeTo;

            DoWList = dateOfWeekList;
            DoWListNotIn = dateOfWeekListNotIn;

            BeginTime = beginTime;
            EndTime = endTime;

            CurrencyIDList = currencyIDList;
            CurrencyIDListNotIn = currencyIDListNotIn;

            AdjustmentType = adjustmentType;
            AdjustmentValue = adjustmentValue;
            AccumulatedBalance = accumulatedBalance;

            Status = status;

            CreatedDTS = createdDTS;
            CreatedBy = createdBy;

            EnabledCriteria = enabledCriteria;
        }

        public int RuleId { get; }
        public string RuleName { get; }
        public int Priority { get; }

        public byte SiteType { get; }
        public string SiteIdList { get; }
        public string SiteIdListNotIn { get; }
        public bool EnabledSiteType { get; set; }
        public string SiteTypeName { get; set; }
        public string SiteNameList { get; set; }
        public string SiteNameListNotIn { get; set; }

        public string CustomerClassList { get; }
        public string CustomerClassListNotIn { get; }
        public bool EnabledCustomerClass { get; set; }

        public string CustomerClassDetailList { get; }
        public string CustomerClassDetailListNotIn { get; }
        public string SportTypeCCList { get; }
        public string SportTypeCCListNotIn { get; }
        public string SportTypeCCNameList { get; set; }
        public string SportTypeCCNameListNotIn { get; set; }
        public bool EnabledCustomerClassDetail { get; set; }

        public decimal VRFrom { get; }
        public decimal VRTo { get; }
        public bool EnabledCurrentVR { get; set; }

        public byte VRStatus { get; }
        public bool EnabledCurrentVRStatus { get; set; }
        public string VRStatusName { get; set; }

        public string SportTypeList { get; }
        public string SportTypeListNotIn { get; }
        public bool EnabledSport { get; set; }
        public string SportTypeNameList { get; set; }
        public string SportTypeNameListNotIn { get; set; }

        public string BetTypeList { get; }
        public string BetTypeListNotIn { get; }
        public bool EnabledBetType { get; set; }
        public string BetTypeNameList { get; set; }
        public string BetTypeNameListNotIn { get; set; }

        public byte LiveIndicator { get; }
        public bool EnabledEvent { get; set; }
        public string LiveIndicatorName { get; set; }

        public string LeagueGroupList { get; }
        public string LeagueGroupListNotIn { get; }
        public string LeagueList { get; }
        public string LeagueListNotIn { get; }
        public bool EnabledLeagueGroup { get; set; }
        public string LeagueGroupNameList { get; set; }
        public string LeagueGroupNameListNotIn { get; set; }

        public string Betfrom { get; }
        public bool EnabledBetfrom { get; set; }
        public string BetfromName { get; set; }

        public decimal StakeFrom { get; }
        public decimal StakeTo { get; }
        public bool EnabledStake { get; set; }

        public string DoWList { get; }
        public string DoWListNotIn { get; }
        public bool EnabledTicketDoW { get; set; }
        public string DoWNameList { get; set; }
        public string DoWNameListNotIn { get; set; }

        public string BeginTime { get; }
        public string EndTime { get; }
        public bool EnabledTicketTime { get; set; }

        public string CurrencyIDList { get; }
        public string CurrencyIDListNotIn { get; }
        public string CurrencyNameList { get; set; }
        public string CurrencyNameListNotIn { get; set; }
        public bool EnabledTicketCur { get; set; }

        public byte AdjustmentType { get; }
        public decimal AdjustmentValue { get; }
        public byte AccumulatedBalance { get; }
        public string AdjustmentCell { get; set; }

        public byte Status { get; }

        public string StatusName { get; set; }

        public DateTime CreatedDTS { get; }

        public string CreatedDate { get; set; }

        public string CreatedBy { get; }

        public string EnabledCriteria { get; }
    }
}
