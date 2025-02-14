namespace Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;

    public class ValidateGlobalSettingCriteria : CriteriaBase
    {
        public ValidateGlobalSettingCriteria(short sportType, IEnumerable<GlobalSettingExcel> globalSettings)
        {
            SportType = sportType;
            GlobalSettings = globalSettings;
        }

        public short SportType { get; }

        public string GlobalSetting => ToXml();

        [SpParam(Ignored = true)]
        public IEnumerable<GlobalSettingExcel> GlobalSettings { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingValidation";

        public override bool IsValid()
            => true;

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
