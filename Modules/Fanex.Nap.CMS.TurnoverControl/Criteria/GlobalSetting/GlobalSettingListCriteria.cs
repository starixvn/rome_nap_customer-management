using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class GlobalSettingListCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public short SportType { get; set; }

        [SpParam(Ignored = true)]
        public string SortField { get; set; }

        [SpParam(Ignored = true)]
        public string SortDirection { get; set; }

        [SpParam(Ignored = true)]
        public int Page { get; set; }

        [SpParam(Ignored = true)]
        public int Size { get; set; }

        [SpParam(Ignored = true)]
        public bool Cache { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_GlobalSetting_List";
        }

        public override bool IsValid()
        {
            Enums.SportType type = (Enums.SportType)SportType;

            return (type == Enums.SportType.Soccer || type == Enums.SportType.Others) &&
                !string.IsNullOrWhiteSpace(SortField) &&
                !string.IsNullOrWhiteSpace(SortDirection);
        }
    }
}
