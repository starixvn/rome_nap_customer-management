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
                xml.Append(GetRowXML(sport, canSetAutoSuspendOnAllLevels));

                foreach (var game in sport.Childs)
                {
                    xml.Append(GetRowXML(game, canSetAutoSuspendOnAllLevels));

                    foreach (var betTypeGroup in game.Childs)
                    {
                        xml.Append(GetRowXML(betTypeGroup, canSetAutoSuspendOnAllLevels));

                        if (betTypeGroup.Childs != null)
                        {
                            foreach (var betType in betTypeGroup.Childs)
                            {
                                xml.Append(GetRowXML(betType, true));
                            }
                        }
                    }
                }
            }

            xml.Append("</Root>");

            return xml.ToString();
        }

        private string GetRowXML(VirtualRateDetailsByGroup rate, bool isBetType = false)
        {
            if (IsMember && isBetType)
            {
                if (rate.LiveId == 0
                    && rate.NonliveId == 0
                    && rate.DisableNonliveAutoSuspend
                    && rate.DisableLiveAutoSuspend)
                {
                    return string.Empty;
                }

                return $"<Row LevelID=\"{rate.LevelId}\" ValueIDLive=\"{rate.LiveId}\" ValueIDNonLive=\"{rate.NonliveId}\"" +
                    (rate.DisableNonliveAutoSuspend ? string.Empty : " IsNonLiveAutoSuspend=\"" + (rate.NonliveAutoSuspend ? 1 : 0) + "\"") +
                    (rate.DisableLiveAutoSuspend ? string.Empty : " IsLiveAutoSuspend=\"" + (rate.LiveAutoSuspend ? 1 : 0) + "\"") + " />";
            }

            if (rate.LiveId == 0 && rate.NonliveId == 0)
            {
                return string.Empty;
            }

            return $"<Row LevelID=\"{rate.LevelId}\" ValueIDLive=\"{rate.LiveId}\" ValueIDNonLive=\"{rate.NonliveId}\" />";
        }
    }
}
