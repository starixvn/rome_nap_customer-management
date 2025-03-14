namespace Fanex.NAP.CMS.BetLimitControl.Request.BetLimitIncrease
{
    public class AddBetLimitIncreaseRequest
    {
        public string Username { get; set; }

        public decimal Multiplier { get; set; }

        public string Note { get; set; }
    }
}
