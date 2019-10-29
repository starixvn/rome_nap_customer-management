using Fanex.Nap.CMS.Customers.Constants;
using Fanex.Nap.CMS.Customers.Resources;

namespace Fanex.Nap.CMS.Customers.Models.PossessStatistics
{
    public class PossessStatisticsModel
    {
        public PossessStatisticsModel(
            int no,
            int custId,
            int totalSB,
            int totalNS,
            string superUsername,
            int superSB,
            int superNS,
            string masterUsername,
            int masterSB,
            int masterNS,
            string agentUsername,
            int agentSB,
            int agentNS,
            byte status,
            int dangerLevel,
            bool monitored)
        {
            No = no;
            CustId = custId;
            TotalSB = totalSB;
            TotalNS = totalNS;

            SuperUsername = superUsername;
            SuperSB = superSB;
            SuperNS = superNS;

            MasterUsername = masterUsername;
            MasterSB = masterSB;
            MasterNS = masterNS;

            AgentUsername = agentUsername;
            AgentSB = agentSB;
            AgentNS = agentNS;

            Status = status;
            DangerLevel = dangerLevel;
            Monitored = monitored;
        }

        public int No { get; }

        public int CustId { get; }

        public int TotalSB { get; }

        public int TotalNS { get; }

        public string SuperUsername { get; }

        public int SuperSB { get; }

        public int SuperNS { get; }

        public string MasterUsername { get; }

        public int MasterSB { get; }

        public int MasterNS { get; }

        public string AgentUsername { get; }

        public int AgentSB { get; }

        public int AgentNS { get; }

        public byte Status { get; }

        public string StatusName => AccountStatusConst.AccountStatusList[Status];

        public int DangerLevel { get; }

        public bool Monitored { get; }

        public string MonitoredText => Monitored ? PossessStatisticsResource.Yes : PossessStatisticsResource.No;
    }
}