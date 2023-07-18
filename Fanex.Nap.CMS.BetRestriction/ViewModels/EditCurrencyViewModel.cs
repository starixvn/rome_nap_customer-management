namespace Fanex.Nap.CMS.BetRestriction.ViewModels
{
    public class EditCurrencyViewModel
    {
        public int ExchangeId { get; set; }

        public string Currency { get; set; }

        public int Type { get; set; }

        public short? LiveBetAccept { get; set; }

        public byte? SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }

        public byte AccumulateBalance { get; set; }
    }
}
