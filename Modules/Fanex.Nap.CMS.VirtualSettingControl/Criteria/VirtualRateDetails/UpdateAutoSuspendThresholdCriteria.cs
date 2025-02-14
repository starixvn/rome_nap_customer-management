using System.Text;
using Fanex.Nap.CMS.Infrastructure.Criteria;
using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    public class UpdateAutoSuspendThresholdCriteria : CommandBaseCriteria
    {
        private const int OneHunded = 100;

        public UpdateAutoSuspendThresholdCriteria(
            int sportType,
            VirtualRateThresholdByGroup[] sports,
            int userId,
            string ip)
        {
            SportType = sportType;
            Sports = sports;
            UserId = userId;
            IP = ip;
        }

        public int SportType { get; }

        public VirtualRateThresholdByGroup[] Sports { get; }

        public int UserId { get; }

        public string IP { get; }

        public string XML => BuildXML();

        public override string GetSettingKey()
            => "Acc_VR_AutoSuspendThreshold_Upd";

        public override bool IsValid()
            => true;

        private string BuildXML()
        {
            var xmlBuilder = new StringBuilder("<Root>");

            foreach (var sport in Sports)
            {
                xmlBuilder.Append(GetRowXML(sport));

                foreach (var game in sport.Childs)
                {
                    xmlBuilder.Append(GetRowXML(game));

                    foreach (var betTypeGroup in game.Childs)
                    {
                        xmlBuilder.Append(GetRowXML(betTypeGroup));

                        if (betTypeGroup.Childs != null)
                        {
                            foreach (var betType in betTypeGroup.Childs)
                            {
                                xmlBuilder.Append(GetRowXML(betType));
                            }
                        }
                    }
                }
            }

            xmlBuilder.Append("</Root>");

            return xmlBuilder.ToString();
        }

        private string GetRowXML(VirtualRateThresholdByGroup rate)
            => string.Format("<Row LevelID=\"{0}\" VRAutoSuspendThresholdLive=\"{1}\" VRAutoSuspendThresholdNonLive=\"{2}\"/>",
                rate.LevelId,
                ConvertToMultiplier(rate.LiveAutoSuspendThreshold),
                ConvertToMultiplier(rate.NonliveAutoSuspendThreshold));

        private static decimal ConvertToMultiplier(decimal value)
            => (value / OneHunded) + 1;
    }
}
