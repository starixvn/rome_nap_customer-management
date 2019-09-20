namespace Fanex.Nap.CMS.BetRestriction.Requests
{
    public class CurrencyRequest
    {
        public int ExchangeId { get; set; }

        public int Type { get; set; }

        public int NewType { get; set; }

        public short? LiveBetAccept { get; set; }

        public int? SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }
    }
}
