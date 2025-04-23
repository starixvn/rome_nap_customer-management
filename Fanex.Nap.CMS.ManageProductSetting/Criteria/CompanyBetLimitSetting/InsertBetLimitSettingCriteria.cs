namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    public class InsertBetLimitSettingCriteria : ResultQueryCommand
    {
        public string InputData { get; set; }
        public int UserID { get; set; }
        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_GlobalStakeLevelGroup_Ins";
    }
}