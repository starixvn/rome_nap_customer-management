using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting
{
    public class AddMultipleGlobalSettingCriteria : NonQueryCriteria
    {
        public AddMultipleGlobalSettingCriteria(
            IEnumerable<GlobalSettingCustomer> globalSettings,
            short sportType,
            int userId,
            string ip) : base(false, sportType, userId, ip)
        {
            GlobalSettings = globalSettings;
        }

        public IEnumerable<GlobalSettingCustomer> GlobalSettings { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingInsertMultiple";

        public override Hashtable GetSpParams()
            => new Hashtable
            {
                ["GlobalSetting"] = ToXml(),
                [nameof(UserId)] = UserId,
                [nameof(IP)] = IP,
                [nameof(ErrCode)] = ErrCode,
                [nameof(ErrMsg)] = ErrMsg
            };

        private string ToXml()
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in GlobalSettings)
            {
                foreach (var mto in item.MaxTurnovers)
                {
                    xmlBuilder.Append(ToXml(item, mto));
                }
            }

            xmlBuilder.Append("</ROOT>");

            return xmlBuilder.ToString();
        }

        private string ToXml(GlobalSettingCustomer item, MaxTurnoverItem mto)
        {
            var builder = new StringBuilder("<gs ")
                .Append("us=\"").Append(item.Username).Append("\" ")
                .Append("isGH=\"").Append(GetIsGroupHeadlineValue(item.Level)).Append("\" ")
                .Append("maxTO=\"").Append(mto.MaxTurnover).Append("\" ")
                .Append("spt=\"").Append(SportType).Append("\" ")
                .Append("pfID=\"").Append(mto.ProfileId).Append("\"")
                .Append("/>");

            return builder.ToString();
        }

        private static int GetIsGroupHeadlineValue(string level)
            => !string.IsNullOrEmpty(level) && level.Equals("headline", StringComparison.OrdinalIgnoreCase)
            ? 1
            : 0;
    }
}
