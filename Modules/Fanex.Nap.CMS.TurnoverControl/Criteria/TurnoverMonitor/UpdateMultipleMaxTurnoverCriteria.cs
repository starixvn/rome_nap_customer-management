namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.TurnoverControl.Models;

    public class UpdateMultipleMaxTurnoverCriteria : CommandBaseCriteria
    {
        public UpdateMultipleMaxTurnoverCriteria(
            IEnumerable<UpdateMaxTurnoverParam> globalSettings,
            int userId,
            string ip)
        {
            GlobalSettings = globalSettings;
            UserId = userId;
            IP = ip;
        }

        [SpParam(Ignored = true)]
        public IEnumerable<UpdateMaxTurnoverParam> GlobalSettings { get; }

        public string GlobalSetting => ToXml();

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_MatchTurnoverUpdateMultiple";

        private string ToXml()
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in GlobalSettings)
            {
                xmlBuilder.Append(ToXml(item));
            }

            xmlBuilder.Append("</ROOT>");

            return xmlBuilder.ToString();
        }

        private static string ToXml(UpdateMaxTurnoverParam item)
            => string.Format(
                "<gs custID=\"{0}\" mid=\"{1}\" isGH=\"{2}\" maxTO=\"{3}\" spt=\"{4}\" pfID=\"{5}\"/>",
                item.CustId,
                item.MatchId,
                item.IsGroupHeadline ? 1 : 0,
                item.MaxTurnover,
                item.SportType,
                item.ProfileId);

        public override bool IsValid()
            => true;
    }
}
