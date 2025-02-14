using System;
using System.Collections;
using System.Collections.Generic;
namespace Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting
{
    using System.Text;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

    public class AddMultipleGlobalSettingCriteria : CommandBaseCriteria
    {
        public AddMultipleGlobalSettingCriteria(
            IEnumerable<GlobalSettingCustomer> globalSettings,
            short sportType,
            int userId,
            string ip)
        {
            SportType = sportType;
            UserId = userId;
            IP = ip;
            GlobalSettings = globalSettings;
        }

        [SpParam(Ignored = true)]
        public short SportType { get; }

        public int UserId { get; }

        public string IP { get; }

        [SpParam(Ignored = true)]
        public IEnumerable<GlobalSettingCustomer> GlobalSettings { get; }
        public string GlobalSetting => ToXml();

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingInsertMultiple";

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
