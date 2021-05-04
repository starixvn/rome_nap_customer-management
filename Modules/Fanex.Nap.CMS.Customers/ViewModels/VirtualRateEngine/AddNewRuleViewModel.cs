namespace Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine
{
    public class AddNewRuleViewModel
    {
        public string RuleTitle { get; set; }

        public int PriorityOrder { get; set; }

        // [1]
        public byte SiteType { get; set; }
        public string Site { get; set; }

        // [2]
        public string CustomerClass { get; set; }

        // [3]
        public decimal VRFrom { get; set; }
        public decimal VRTo { get; set; }

        // [4]
        public byte VRStatus { get; set; }

        // [5]
        public string Sport { get; set; }

        // [6]
        public string BetType { get; set; }

        // [7]
        public byte LiveIndicator { get; set; }

        // [8]
        public string LeagueGroup { get; set; }
        public string League { get; set; }

        // [9]
        public byte Betfrom { get; set; }

        // [10]
        public decimal StakeFrom { get; set; }
        public decimal StakeTo { get; set; }

        // [11]
        public string DateOfWeek { get; set; }

        // [12]
        public string BeginTime { get; set; }
        public string EndTime { get; set; }

        public byte AdjustmentType { get; set; }

        public decimal AdjustmentValue { get; set; }

        public byte Active { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }
    }
}
