namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class EditGlobalSettingCriteria : NonQueryCriteria
    {
        public int CustId { get; set; }

        public decimal MaxTurnover { get; set; }

        public bool IsGroupHeadline { get; set; }

        public short SportType { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_GlobalSetting_Edit";
        }

        public override bool IsValid()
        {
            Enums.SportType type = (Enums.SportType)SportType;
            bool validSportType = type == Enums.SportType.Soccer || type == Enums.SportType.Others;
            bool validCustId = CustId > 0;
            bool validMaxTurnover = 0 <= MaxTurnover && MaxTurnover <= int.MaxValue;

            return validCustId && validMaxTurnover && validSportType;
        }
    }
}
