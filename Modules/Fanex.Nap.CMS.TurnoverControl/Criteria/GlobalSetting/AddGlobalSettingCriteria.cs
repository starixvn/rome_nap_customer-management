using System.Collections;
using System.Text;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class AddGlobalSettingCriteria : NonQueryCriteria
    {
        public AddGlobalSettingCriteria(
            string username,
            MaxTurnoverItem[] maxTurnovers,
            bool isGroupHeadline,
            short sportType,
            int userId,
            string ip) : base(isGroupHeadline, sportType, userId, ip)
        {
            Username = username;
            MaxTurnovers = maxTurnovers;
        }

        public string Username { get; }

        public MaxTurnoverItem[] MaxTurnovers { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingInsert";

        public override Hashtable GetSpParams()
            => new Hashtable
            {
                [nameof(Username)] = Username,
                ["GlobalSetting"] = ToXml(),
                [nameof(IsGroupHeadline)] = IsGroupHeadline,
                [nameof(SportType)] = SportType,
                [nameof(UserId)] = UserId,
                [nameof(IP)] = IP,
                [nameof(ErrCode)] = ErrCode,
                [nameof(ErrMsg)] = ErrMsg,
            };

        private string ToXml()
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in MaxTurnovers)
            {
                xmlBuilder.Append(ToXml(item));
            }

            xmlBuilder.Append("</ROOT>");

            return xmlBuilder.ToString();
        }

        private static string ToXml(MaxTurnoverItem item)
            => $"<GlobalSetting MaxTurnover=\"{item.MaxTurnover}\" ProfileID=\"{item.ProfileId}\"/>";
    }
}
