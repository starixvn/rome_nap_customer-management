namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    internal class GetDuplicatedSiteSettingCriteria : CommandBaseCriteria
    {
        protected GetDuplicatedSiteSettingCriteria()
        {
        }

        public GetDuplicatedSiteSettingCriteria(AssignSiteSettingRequest request)
            => PolicySites = ToXml(request);

        public string PolicySites { get; set; }

        public bool IsTest { get; } = AppSettings.IsTest;

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
