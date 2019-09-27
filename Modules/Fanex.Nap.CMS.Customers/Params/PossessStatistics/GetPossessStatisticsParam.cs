namespace Fanex.Nap.CMS.Customers.Params.PossessStatistics
{
    public class GetPossessStatisticsParam
    {
        public GetPossessStatisticsParam(
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

        public byte SBFrom { get; }

        public byte SBTo { get; }

        public byte NSFrom { get; }

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
    }
}