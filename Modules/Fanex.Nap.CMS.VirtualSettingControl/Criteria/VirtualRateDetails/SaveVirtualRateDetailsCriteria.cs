namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using System.Collections;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public class SaveVirtualRateDetailsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SportType { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public VirtualRateDetailsReport Rates { get; set; }

        public override string GetSettingKey()
            => "Acc_CustVirtualMultiplier_Upd";

        public override Hashtable GetSpParams()
        {
            var result = new Hashtable();
            result.Add("custid", CustId);
            result.Add("sporttype", SportType);
            result.Add("userid", UserId);
            result.Add("ip", IP);
            result.Add("errcode", 0);
            result.Add("errmsg", string.Empty);

            var xml = "<Root>";

            foreach (var sport in Rates.Sports)
            {
                xml += GetRowXML(sport);

                foreach (var game in sport.Childs)
                {
                    xml += GetRowXML(game);

                    foreach (var betTypeGroup in game.Childs)
                    {
                        xml += GetRowXML(betTypeGroup);

                        foreach (var betType in betTypeGroup.Childs)
                        {
                            xml += GetRowXML(betType);
                        }
                    }
                }
            }

            xml += "</Root>";
            result.Add("virtualmultiplier", xml);

            return result;
        }

        private string GetRowXML(VirtualRateDetailsByGroup rate)
            => rate.LiveId == 0 && rate.NonliveId == 0
            ? string.Empty
            : $"<Row LevelID=\"{rate.LevelId}\" ValueIDLive=\"{rate.LiveId}\" ValueIDNonLive=\"{rate.NonliveId}\" />";
    }
}
