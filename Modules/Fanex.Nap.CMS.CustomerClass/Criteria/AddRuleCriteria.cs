namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using System;
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class AddRuleCriteria : CommandCriteriaBase
    {
        public AddRuleCriteria(AddRuleRequest request, int ruleId = 0)
            : base(request.UserId, request.IP)
            => Rules = ToXml(request, ruleId);

        public string Rules { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassRuleIns";

        private static string ToXml(AddRuleRequest request, int ruleId)
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in request.CCRuleTitles)
            {
                xmlBuilder.Append("<r");
                if (ruleId != 0)
                {
                    xmlBuilder.Append(" RuleID=\"").Append(ruleId).Append("\"");
                }

                xmlBuilder
                    .Append(" CustomerClass=\"").Append(item.CustomerClass == null ? string.Empty : item.CustomerClass.ToString()).Append("\"")
                    .Append(" CustomerClassDetail=\"").Append(item.CustomerClassDetail == null ? string.Empty : item.CustomerClassDetail.ToString()).Append("\"")
                    .Append(" RuleTitle=\"").Append(item.RuleTitle).Append("\"")
                    .Append(" SportType=\"").Append(request.SportType).Append("\"")
                    .Append(" LeagueGroupIDs=\"").Append(request.LeagueGroups).Append("\"")
                    .Append(" IsLive=\"").Append(request.IsLive).Append("\"")
                    .Append(" DiffBalanceRatio=\"").Append(request.DiffBalanceRatio).Append("\"")
                    .Append(" BetDelay=\"").Append(request.BetDelay).Append("\"")
                    .Append(" NextBetInterval=\"").Append(request.NextBetIntervalSec).Append("\"")
                    .Append(" SuperBetInterval=\"").Append(request.SuperBetIntervalSec).Append("\"")
                    .Append(" SiteBetInterval=\"").Append(request.SiteBetIntervalSec).Append("\"")
                    .Append(" MinBet=\"").Append(request.MinBet).Append("\"")
                    .Append(" MaxBet=\"").Append(request.MaxBet).Append("\"")
                    .Append(" MaxPerMatch=\"").Append(request.MaxPerMatch).Append("\"")
                    .Append(" RepeatedBets=\"").Append(request.RepeatedBets).Append("\"")
                    .Append(" SeizeBetTicket=\"").Append(request.SeizeBet).Append("\"")
                    .Append(" DisableParlay=\"").Append(Convert.ToByte(request.DisableParlay)).Append("\"")
                    .Append(" PeakHourSpreadAdjustment=\"").Append(request.PeakHourSpread).Append("\"/>");
            }

            xmlBuilder.Append("</ROOT>");
            return xmlBuilder.ToString();
        }
    }
}
