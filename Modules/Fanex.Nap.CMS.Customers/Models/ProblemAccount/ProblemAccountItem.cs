namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    using System;
    using System.Collections.Generic;

    public class ProblemAccountItem
    {
        public ProblemAccountItem(
            int custId,
            string username,
            string headline,
            string super,
            string master,
            string agent,
            bool isProblemAccSuper,
            bool isProblemAccMaster,
            bool isProblemAccAgent,
            bool isProblemAcc,
            decimal? superMPM,
            decimal? superMaxBet,
            decimal? masterMPM,
            decimal? masterMaxBet,
            decimal? agentMPM,
            decimal? agentMaxBet,
            decimal? memberMPM,
            decimal? memberMaxBet,
            byte status,
            DateTime markDate,
            string comment,
            int? abnormalBetNumber,
            string firstWeekInformed,
            string secondWeekInformed,
            string currency)
        {
            CustId = custId;
            Username = username;
            Headline = headline;
            Super = super;
            Master = master;
            Agent = agent;
            IsProblemAccSuper = isProblemAccSuper;
            IsProblemAccMaster = isProblemAccMaster;
            IsProblemAccAgent = isProblemAccAgent;
            IsProblemAcc = isProblemAcc;
            SuperMPM = superMPM;
            SuperMaxBet = superMaxBet;
            MasterMPM = masterMPM;
            MasterMaxBet = masterMaxBet;
            AgentMPM = agentMPM;
            AgentMaxBet = agentMaxBet;
            MPM = memberMPM;
            MaxBet = memberMaxBet;
            CustStatus = status;
            MarkDate = markDate;
            Comment = comment;
            AbnormalBetNumber = abnormalBetNumber;
            FirstWeekInformed = firstWeekInformed;
            SecondWeekInformed = secondWeekInformed;
            Currency = currency;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Headline { get; }

        public string Super { get; }

        public string Master { get; }

        public string Agent { get; }

        public bool IsProblemAccSuper { get; }

        public bool IsProblemAccMaster { get; }

        public bool IsProblemAccAgent { get; }

        public bool IsProblemAcc { get; }

        public decimal? SuperMPM { get; }

        public decimal? SuperMaxBet { get; }

        public decimal? MasterMPM { get; }

        public decimal? MasterMaxBet { get; }

        public decimal? AgentMPM { get; }

        public decimal? AgentMaxBet { get; }

        public decimal? MPM { get; }

        public decimal? MaxBet { get; }

        public byte CustStatus { get; }

        public DateTime MarkDate { get; }

        public string Comment { get; }

        public int? AbnormalBetNumber { get; }

        public string FirstWeekInformed { get; }

        public string SecondWeekInformed { get; }

        public string Currency { get; }

        public string StatusName { get; set; }

        public string MarkDateString { get; set; }

        public IEnumerable<string> Reasons { get; set; }
    }
}
