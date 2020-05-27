namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    using Fanex.Data.Repository;

    public class InsertCriteria : CriteriaBase
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

        public string IP { get; set; }

        public int UserId { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override bool IsValid() => !string.IsNullOrWhiteSpace(BetLimitType) &&
            !string.IsNullOrWhiteSpace(ProductList) &&
            !string.IsNullOrWhiteSpace(SportTypeList) &&
            !string.IsNullOrWhiteSpace(BetTypeList) &&
            UserId > 0;

        public override string GetSettingKey() => "NAP_BLControl_TypeInsert";
    }
}
