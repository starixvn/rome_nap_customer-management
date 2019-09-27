namespace Fanex.Nap.CMS.Customers.Criteria.PossessStatistics
{
    using Data.Repository;

    public class GetPossessStatisticsCriteria : CriteriaBase
    {
        public GetPossessStatisticsCriteria(
            byte sbFrom,
            byte sbTo,
            byte nsFrom,
            byte nsTo,
            string username,
            string agentStatus,
            byte dangerLevel,
            byte monitorPT,
            byte sortBy,
            string filterPossess,
            string filterDanger,
            int pageIndex,
            int pageSize)
        {
            SBFrom = sbFrom;
            SBTo = sbTo;
            NSFrom = nsFrom;
            NSTo = nsTo;
            Username = username;
            AgentStatus = agentStatus;
            DangerLevel = dangerLevel;
            MonitorPT = monitorPT;
            SortBy = sortBy;
            FilterPossess = filterPossess;
            FilterDanger = filterDanger;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        [SpParam(SqlParamName = "SB_from")]
        public byte SBFrom { get; }

        [SpParam(SqlParamName = "SB_to")]
        public byte SBTo { get; }

        [SpParam(SqlParamName = "NS_from")]
        public byte NSFrom { get; }

        [SpParam(SqlParamName = "NS_to")]
        public byte NSTo { get; }

        public string Username { get; }

        public string AgentStatus { get; }

        public byte DangerLevel { get; }

        public byte MonitorPT { get; }

        public byte SortBy { get; }

        public string FilterPossess { get; }

        public string FilterDanger { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; set; }

        public override string GetSettingKey() => "CS_Customer_PossessStatistics_Sel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(AgentStatus)
            && !string.IsNullOrWhiteSpace(FilterPossess)
            && !string.IsNullOrWhiteSpace(FilterDanger);
    }
}