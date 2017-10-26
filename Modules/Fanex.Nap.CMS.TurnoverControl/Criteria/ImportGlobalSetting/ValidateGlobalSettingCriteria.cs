using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Nap.CMS.TurnoverControl.Enums;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting
{
    public class ValidateGlobalSettingCriteria : CriteriaBase
    {
        public ValidateGlobalSettingCriteria(
            SportType sportType,
            IEnumerable<GlobalSettingExcel> globalSettingExcels)
        {
            Type = (short)sportType;
            GlobalSettingExcels = globalSettingExcels;
        }

        [SpParam(Ignored = true)]
        public short Type { get; }

        [SpParam(Ignored = true)]
        public IEnumerable<GlobalSettingExcel> GlobalSettingExcels { get; }

        public override string GetSettingKey()
        {
            return "NAP_TOControl_GlobalSettingValidation";
        }

        public override bool IsValid()
        {
            SportType sportType = (SportType)Type;
            bool validSportType = sportType == SportType.Soccer || sportType == SportType.Others;
            bool validList = GlobalSettingExcels != null && GlobalSettingExcels.Any();

            return validSportType && validList;
        }

        public override Hashtable GetSpParams()
        {
            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in GlobalSettingExcels)
            {
                xmlBuilder.Append(ToXml(item));
            }

            xmlBuilder.Append("</ROOT>");

            return new Hashtable
            {
                { "GlobalSetting", xmlBuilder.ToString() }
            };
        }

        private string ToXml(GlobalSettingExcel item)
        {
            return string.Format(
                "<GlobalSetting Username=\"{0}\" IsGroupHeadline=\"{1}\" MaxTurnover=\"{2}\" SportType=\"{3}\"/>",
                item.UserName,
                item.Headline,
                item.MaxTurnover,
                Type);
        }
    }
}
