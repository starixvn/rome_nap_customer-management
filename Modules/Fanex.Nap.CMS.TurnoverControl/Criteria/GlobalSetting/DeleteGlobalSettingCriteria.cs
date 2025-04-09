namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class DeleteGlobalSettingCriteria : NonQueryCriteria
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingDelete";
    }
}
