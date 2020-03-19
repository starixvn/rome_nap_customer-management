namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using System.Collections;
    using System.Text;
    using Fanex.Data.Repository;
    using Models.VirtualRateDetails;

    public class UpdateAutoSuspendThresholdCriteria : CriteriaBase
    {
        private const int OneHunded = 100;

        public UpdateAutoSuspendThresholdCriteria(
            VirtualRateThresholdByGroup[] sports,
            int userId,
            string ip)
        {
            Sports = sports;
            UserId = userId;
            IP = ip;
        }

        public VirtualRateThresholdByGroup[] Sports { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
            => "Acc_VR_AutoSuspendThreshold_Upd";

        public override Hashtable GetSpParams()
            => new Hashtable
            {
                ["xml"] = BuildXML(),
                ["userid"] = UserId,
                ["ip"] = IP,
                ["errcode"] = 0,
                ["errmsg"] = string.Empty
            };

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

                        foreach (var betType in betTypeGroup.Childs)
                        {
                            xmlBuilder.Append(GetRowXML(betType));
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
