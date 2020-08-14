namespace Fanex.NAP.CMS.BetLimitTypeControl.Requests
{
    public class InsertRequest
    {
        public string BetLimitType { get; set; }

        public string ProductList { get; set; }

        public string SportTypeList { get; set; }

        public string BetTypeList { get; set; }

        public short LiveIndicator { get; set; }

        public string LeagueList { get; set; }

        public string BeginTime { get; set; }

        public string EndTime { get; set; }

        public bool RestrictVIP { get; set; }
    }
}
