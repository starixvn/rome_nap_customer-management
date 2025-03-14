namespace Fanex.NAP.CMS.BetLimitControl.Models.BetLimitIncrease
{
    using System;

    public class BetLimitIncreaseModel
    {
        public int CustId { get; set; }
        public string Username { get; set; }
        public int SiteId { get; set; }
        public string Site { get; set; }
        public string Currency { get; set; }
        public decimal Multiplier { get; set; }
        public int SportType { get; set; }
        public int BetType { get; set; }
        public string Note { get; set; }
        public string LastModifiedUser { get; set; }
        public string LastModifiedTime { get; set; }
    }
}
