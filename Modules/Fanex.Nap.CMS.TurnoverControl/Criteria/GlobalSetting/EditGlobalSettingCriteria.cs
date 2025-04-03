namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class EditGlobalSettingCriteria : NonQueryCriteria
    {
        public int CustId { get; set; }

        public int ProfileId { get; set; }

        public decimal MaxTurnover { get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingUpdate";
    }
}
