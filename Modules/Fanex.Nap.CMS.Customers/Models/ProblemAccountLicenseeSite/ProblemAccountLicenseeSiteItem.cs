namespace Fanex.Nap.CMS.Customers.Models.ProblemAccountLicenseeSite
{
    using System;
    using System.Collections.Generic;

    public class ProblemAccountLicenseeSiteItem
    {
        public ProblemAccountLicenseeSiteItem(
            int custId,
            string site,
            bool isAcknowledgement,
            string username,
            string username2,
            string currency,
            bool isVIP,
            string actionName,
            decimal? maxBet,
            decimal? maxPerMatch,
            DateTime markDate,
            string comment,
            string inform1,
            string inform2,
            int? abnormalBetNumber)
        {
            CustId = custId;
            Site = site;
            IsAcknowledgement = isAcknowledgement;
            Username = username;
            Username2 = username2;
            Currency = currency;
            IsVIP = isVIP;
            ActionName = actionName;
            MaxBet = maxBet;
            MaxPerMatch = maxPerMatch;
            MarkDate = markDate;
            Comment = comment;
            Inform1 = inform1;
            Inform2 = inform2;
            AbnormalBetNumber = abnormalBetNumber;
        }

        public int CustId { get; }

        public string Site { get; }

        public bool IsAcknowledgement { get; }

        public string Username { get; }

        public string Username2 { get; }

        public string Currency { get; }

        public bool IsVIP { get; }

        public string ActionName { get; }

        public decimal? MaxBet { get; }

        public decimal? MaxPerMatch { get; }

        public DateTime MarkDate { get; }

        public string MarkDateString
            => MarkDate.ToString("MM/dd/yyyy HH:mm:ss");

        public string Comment { get; }

        public string Inform1 { get; }

        public string Inform2 { get; }

        public int? AbnormalBetNumber { get; }

        public IEnumerable<string> Reasons { get; set; }
    }
}
