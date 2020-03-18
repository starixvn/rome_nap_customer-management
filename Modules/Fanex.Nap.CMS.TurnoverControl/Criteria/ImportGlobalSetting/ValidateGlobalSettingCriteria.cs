using System.Collections;
using System.Collections.Generic;
using System.Text;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting
{
    public class ValidateGlobalSettingCriteria : CriteriaBase
    {
        public ValidateGlobalSettingCriteria(short sportType, IEnumerable<GlobalSettingExcel> globalSettings)
        {
            SportType = sportType;
            GlobalSettings = globalSettings;
        }

        public short SportType { get; }

        public IEnumerable<GlobalSettingExcel> GlobalSettings { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingValidation";

        public override Hashtable GetSpParams()
            => new Hashtable
            {
                [nameof(SportType)] = SportType,
                ["GlobalSetting"] = ToXml()
            };

        private string ToXml()
        {
            var builder = new StringBuilder();
            builder.Append("<ROOT>");

            foreach (var item in GlobalSettings)
            {
                foreach (var mto in item.MaxTurnovers)
                {
                    builder.Append(ToXml(item, mto));
                }
            }

            builder.Append("</ROOT>");

            return builder.ToString();
        }

        private string ToXml(GlobalSettingExcel globalSetting, MaxTurnoverExcel mto)
        {
            var builder = new StringBuilder("<gs ")
                .Append("us=\"").Append(globalSetting.Username).Append("\" ")
                .Append("isGH=\"").Append(globalSetting.Headline).Append("\" ")
                .Append("maxTO=\"").Append(mto.MaxTurnover).Append("\" ")
                .Append("spt=\"").Append(SportType).Append("\" ")
                .Append("pfID=\"").Append(mto.ProfileId).Append("\" ")
                .Append("rowID=\"").Append(globalSetting.ExcelRow).Append("\"")
                .Append("/>");

            return builder.ToString();
        }
    }
}
