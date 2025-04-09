namespace Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting
{
    using Fanex.Data.Repository;

    public class ValidateGlobalSettingCriteria : CriteriaBase
    {
        public short SportType { get; set; }

        public string GlobalSetting {  get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingValidation";

        public override bool IsValid()
            => true;
    }
}
