namespace Fanex.NAP.CMS.BetLimitTypeControl.Requests
{
    public class SearchRequest
    {
        public byte ProductId { get; set; }

        public short SportTypeId { get; set; }

        public short BetTypeId { get; set; }

        public byte LiveIndicator { get; set; }
    }
}
