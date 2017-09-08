using System.Collections;
using System.Linq;
using System.Text;
using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class UpdateMultipleMaxTurnoverCriteria : NonQueryCriteria
    {
        [SpParam(Ignored = true)]
        public UpdateMaxTurnoverViewModel[] GlobalSettingItems { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_TOControl_MatchTurnoverUpdateMultiple";
        }

        public override bool IsValid()
        {
            return GlobalSettingItems != null && GlobalSettingItems.Any();
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

        private static string ToXml(UpdateMaxTurnoverViewModel item)
        {
            return string.Format(
                "<GlobalSetting CustId=\"{0}\" MatchId=\"{1}\" IsGroupHeadline=\"{2}\" MaxTurnover=\"{3}\"/>",
                item.CustId,
                item.MatchId,
                item.IsGroupHeadline ? 1 : 0,
                item.MaxTurnover);
        }
    }
}
