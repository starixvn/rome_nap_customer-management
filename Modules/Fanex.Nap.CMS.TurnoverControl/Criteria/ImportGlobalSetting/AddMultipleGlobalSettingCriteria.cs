using System;
using System.Collections;
using System.Text;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting
{
    public class AddMultipleGlobalSettingCriteria : NonQueryCriteria
    {
        [SpParam(Ignored = true)]
        public short SportType { get; set; }

        [SpParam(Ignored = true)]
        public GlobalSettingItem[] GlobalSettingItems { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_TOControl_GlobalSettingInsertMultiple";
        }

        public override bool IsValid()
        {
            Enums.SportType type = (Enums.SportType)SportType;
            bool validSportType = type == Enums.SportType.Soccer || type == Enums.SportType.Others;
            bool validList = GlobalSettingItems != null && GlobalSettingItems.Length > 0;

            return validSportType && validList;
        }

        public override Hashtable GetSpParams()
        {
            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in GlobalSettingItems)
            {
                xmlBuilder.Append(ToXml(item));
            }

            xmlBuilder.Append("</ROOT>");

            return new Hashtable
            {
                { "GlobalSetting", xmlBuilder.ToString() },
                { "UserId", UserId },
                { "IP", IP },
                { "ErrCode", ErrCode },
                { "ErrMsg", ErrMsg }
            };
        }

        private string ToXml(GlobalSettingItem item)
        {
            bool headline = !string.IsNullOrEmpty(item.Level) &&
                item.Level.Equals("headline", StringComparison.OrdinalIgnoreCase);

            return string.Format(
                "<GlobalSetting Username=\"{0}\" IsGroupHeadline=\"{1}\" MaxTurnover=\"{2}\" SportType=\"{3}\"/>",
                item.UserName,
                headline ? 1 : 0,
                item.MaxTurnover,
                SportType);
        }
    }
}
