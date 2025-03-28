namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    using System;
    using System.Collections.Generic;

    public class ProblemAccountItem
    {
        public ProblemAccountItem()
        {
        }

#pragma warning disable S107 // Methods should not have too many parameters
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
            string currency,
            byte preventiveActionId,
            string voidTime,
            double? limitAmount)
#pragma warning restore S107 // Methods should not have too many parameters
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
            PreventiveActionId = preventiveActionId;
            VoidTime = voidTime;
            LimitAmount = limitAmount;
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Headline { get; set; }

        public string Super { get; set; }

        public string Master { get; set; }

        public string Agent { get; set; }

        public bool IsProblemAccSuper { get; set; }

        public bool IsProblemAccMaster { get; set; }

        public bool IsProblemAccAgent { get; set; }

        public bool IsProblemAcc { get; set; }

        public decimal? SuperMPM { get; set; }

        public decimal? SuperMaxBet { get; set; }

        public decimal? MasterMPM { get; set; }

        public decimal? MasterMaxBet { get; set; }

        public decimal? AgentMPM { get; set; }

        public decimal? AgentMaxBet { get; set; }

        public decimal? MPM { get; set; }

        public decimal? MaxBet { get; set; }

        public byte CustStatus { get; set; }

        public DateTime MarkDate { get; set; }

        public string Comment { get; set; }

        public int? AbnormalBetNumber { get; set; }

        public string FirstWeekInformed { get; set; }

        public string SecondWeekInformed { get; set; }

        public string Currency { get; set; }

        public byte PreventiveActionId { get; set; }

        public string PreventiveAction { get; set; }

        public string StatusName { get; set; }

        public string MarkDateString { get; set; }

        public IEnumerable<string> Reasons { get; set; }

        public string VoidTime { get;}

        public double? LimitAmount { get;}
    }
}
