namespace Fanex.NAP.CMS.BetLimitControl.Models
{
    public class CustomerBetLimitModel
    {
        public int CustID { get; set; }
        public string Username { get; set; }
        public bool VIP { get; set; }
        public string Level { get; set; }
        public string Currency { get; set; }
        public string Site { get; set; }
        public string BetLimitType { get; set; }
        public int BetLimitTypeID { get; set; }
        public decimal? MaxBet { get; set; }
        public decimal? MaxPerMatch { get; set; }
        public byte? Mode { get; set; }
        public decimal? AgentMaxBet { get; set; }
        public decimal? AgentMaxPerMatch { get; set; }
        public byte? AgentMode { get; set; }
        public decimal? MasterMaxBet { get; set; }
        public decimal? MasterMaxPerMatch { get; set; }
        public byte? MasterMode { get; set; }
        public decimal? SuperMaxBet { get; set; }
        public decimal? SuperMaxPerMatch { get; set; }
        public byte? SuperMode { get; set; }
    }
}