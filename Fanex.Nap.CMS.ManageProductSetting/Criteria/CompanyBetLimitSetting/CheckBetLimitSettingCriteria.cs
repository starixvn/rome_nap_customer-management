namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class CheckBetLimitSettingCriteria : ResultQueryCommand
    {
        public int SettingStatus { get => AppSettings.SettingStatus; }
        public string InputData { get; set; }
        public override string GetSettingKey()
            => "NAP_GlobalStakeLevelGroup_Check";
    }
}