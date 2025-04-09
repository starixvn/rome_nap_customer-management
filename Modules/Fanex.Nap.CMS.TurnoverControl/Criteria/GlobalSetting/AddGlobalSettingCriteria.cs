namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class AddGlobalSettingCriteria : NonQueryCriteria
    {
        public string Username { get; set; }
        public string GlobalSetting { get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingInsert";
    }
}
