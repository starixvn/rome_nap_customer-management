namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class GetDuplicatedSiteSettingCriteria : QueryCriteriaBase
    {
        protected GetDuplicatedSiteSettingCriteria()
        {
        }

        public GetDuplicatedSiteSettingCriteria(AssignSiteSettingRequest request)
            => PolicySites = ToXml(request);

        public string PolicySites { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySiteCheck";

        private static string ToXml(AssignSiteSettingRequest request)
        {
            var xmlBuilder = new StringBuilder();

            foreach (var item in request.SiteCurrencies)
            {
                AppendElement(xmlBuilder, request.PolicyId, item.Sites, item.Currencies, item.Platforms);
            }

            return InsertRoot(xmlBuilder.ToString());
        }

        protected static string InsertRoot(string xml)
            => $"<ROOT>{xml}</ROOT>";

        protected static void AppendElement(
            StringBuilder xmlBuilder,
            int policyId,
            string sites,
            string currencies,
            string platforms)
        {
            xmlBuilder
                .Append("<r PolicyID=\"").Append(policyId).Append("\"")
                .Append(" Site=\"").Append(sites).Append("\"")
                .Append(" Currency=\"").Append(currencies).Append("\"")
                .Append(" Platform=\"").Append(platforms).Append("\"/>");
        }
    }
}
