namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using System.Collections.Generic;
    using System.Text;

    internal class GetPagedRuleListCriteria : QueryCriteriaBase
    {
        public GetPagedRuleListCriteria(SearchCustomerClassSettingRequest request, IEnumerable<LeagueGroup> leagueGroups)
        {
            Site = request.Site;
            CustomerClass = request.CustomerClass;
            SportType = request.SportType;
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
            LeagueGroups = BuildLeageGroupXML(leagueGroups);
        }

        public string Site { get; }

        public short CustomerClass { get; }

        public short SportType { get; }

        public string LeagueGroups { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSetting";

        protected string BuildLeageGroupXML(IEnumerable<LeagueGroup> leagueGroups)
        {
            var xmlBuilder = new StringBuilder();

            foreach (var lg in leagueGroups)
            {
                xmlBuilder
                .Append("<r GroupID=\"").Append(lg.Id).Append("\"")
                .Append(" GroupName=\"").Append(lg.Name).Append("\"")
                .Append(" SportType=\"").Append(lg.SportType).Append("\"/>");
            }

            return $"<ROOT>{xmlBuilder.ToString()}</ROOT>";
        }
    }
}
