namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class AddGlobalSettingCriteria : NonQueryCriteria
    {
        public string UserName { get; set; }

        public decimal MaxTurnover { get; set; }

        public bool IsGroupHeadline { get; set; }

        public short SportType { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_GlobalSetting_Add";
        }

        public override bool IsValid()
        {
            Enums.SportType type = (Enums.SportType)SportType;
            bool validSportType = type == Enums.SportType.Soccer || type == Enums.SportType.Others;
            bool validUserName = !string.IsNullOrWhiteSpace(UserName);
            bool validMaxTurnover = 0 <= MaxTurnover && MaxTurnover <= int.MaxValue;

            return validSportType && validUserName && validMaxTurnover;
        }
    }
}
