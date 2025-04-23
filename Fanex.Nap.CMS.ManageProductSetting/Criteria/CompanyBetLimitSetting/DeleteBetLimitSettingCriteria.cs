namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    public class DeleteBetLimitSettingCriteria : ResultQueryCommand
    {
        public string InputData { get; set; }
        public bool IsCredit { get; set; }
        public int UserID { get; set; }
        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_GlobalStakeLevelGroup_Del";
    }
}