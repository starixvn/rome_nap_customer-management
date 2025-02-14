namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;

    internal class GetPagedRuleListCriteria : QueryCriteriaBase
    {
        public GetPagedRuleListCriteria(SearchCustomerClassSettingRequest request, IEnumerable<LeagueGroup> leagueGroups, string sportTypes)
        {
            Site = request.Site;
            CustomerClass = request.CustomerClass ?? string.Empty;
            CustomerClassDetail = request.CustomerClassDetail ?? string.Empty;
            SportTypes = sportTypes;
            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
            LeagueGroups = BuildLeageGroupXML(leagueGroups);
            Currencies = request.Currencies;
        }

        public string Site { get; set; }

        public string CustomerClass { get; set; }

        public string CustomerClassDetail { get; set; }

        public string SportTypes { get; set; }

        public string LeagueGroups { get; set; }

        public string Currencies { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSetting";

        protected string BuildLeageGroupXML(IEnumerable<LeagueGroup> leagueGroups)
        {
            var xmlBuilder = new StringBuilder();

            foreach (var lg in leagueGroups)
            {
                xmlBuilder
                .Append("<r GroupID=\"").Append(lg.Id).Append("\"")
                .Append(" GroupName=\"").Append(WebUtility.HtmlEncode(lg.Name)).Append("\"")
                .Append(" SportType=\"").Append(lg.SportType).Append("\"/>");
            }

            return $"<ROOT>{xmlBuilder.ToString()}</ROOT>";
        }
    }
}
