namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using System.Collections;
    using System.Text;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Constants;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public class SaveVirtualRateDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SportType { get; set; }

        public bool IsManualUpdate { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public VirtualRateDetailsReport Rates { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGlobalSetting { get; set; }

        [SpParam(Ignored = true)]
        public bool IsMember { get; set; }

        [SpParam(Ignored = true)]
        public bool IsMemberCompactMode { get; set; }

        public override string GetSettingKey()
            => "Acc_CustVirtualMultiplier_Upd";

        public override Hashtable GetSpParams()
            => new Hashtable
            {
                ["custid"] = CustId,
                ["sporttype"] = SportType,
                ["ismanualupdate"] = IsManualUpdate,
                ["virtualmultiplier"] = BuildXML(),
                ["userid"] = UserId,
                ["ip"] = IP,
                ["errcode"] = 0,
                ["errmsg"] = string.Empty
            };

        private string BuildXML()
        {
            var canSetAutoSuspendOnAllLevels = SportType == SportTypeConstants.Basketball;

            var xml = new StringBuilder();
            xml.Append("<Root>");

            foreach (var sport in Rates.Sports)
            {
                xml.Append(BuildXMLRow(sport, canSetAutoSuspendOnAllLevels));

                foreach (var game in sport.Childs)
                {
                    xml.Append(BuildXMLRow(game, canSetAutoSuspendOnAllLevels));

                    foreach (var betTypeGroup in game.Childs)
                    {
                        xml.Append(BuildXMLRow(betTypeGroup, canSetAutoSuspendOnAllLevels));

                        if (betTypeGroup.Childs != null)
                        {
                            foreach (var betType in betTypeGroup.Childs)
                            {
                                xml.Append(BuildXMLRow(betType, true));
                            }
                        }
                    }
                }
            }

            xml.Append("</Root>");

            return xml.ToString();
        }

        private string BuildXMLRow(VirtualRateDetailsByGroup rate, bool isBetType)
            => !IsMemberCompactMode && IsMember && isBetType
            ? BuildXMLRowAutoSuspend(rate)
            : BuildXMLRowWithoutAutoSuspend(rate);

        private string BuildXMLRowAutoSuspend(VirtualRateDetailsByGroup rate)
        {
            var xml = new StringBuilder();
            xml.AppendFormat(
                "<Row LevelID=\"{0}\" ValueIDLive=\"{1}\" ValueIDNonLive=\"{2}\" IsOverrideASNonLive=\"{3}\" IsOverrideASLive=\"{4}\"",
                rate.LevelId,
                rate.LiveId,
                rate.NonliveId,
                BoolToInt(rate.IsOverrideASNonlive),
                BoolToInt(rate.IsOverrideASLive));
            if (!rate.DisableNonliveAutoSuspend)
            {
                xml.AppendFormat(" IsNonLiveAutoSuspend=\"{0}\"", BoolToInt(rate.NonliveAutoSuspend));
            }

            if (!rate.DisableLiveAutoSuspend)
            {
                xml.AppendFormat(" IsLiveAutoSuspend=\"{0}\"", BoolToInt(rate.LiveAutoSuspend));
            }

            xml.Append(" />");

            return xml.ToString();
        }

        private static int BoolToInt(bool value) => value ? 1 : 0;

        private static string BuildXMLRowWithoutAutoSuspend(VirtualRateDetailsByGroup rate)
            => rate.LiveId == 0 && rate.NonliveId == 0
            ? string.Empty
            : $"<Row LevelID=\"{rate.LevelId}\" ValueIDLive=\"{rate.LiveId}\" ValueIDNonLive=\"{rate.NonliveId}\" />";
    }
}
