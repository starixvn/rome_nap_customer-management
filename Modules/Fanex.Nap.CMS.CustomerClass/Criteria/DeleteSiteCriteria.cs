namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using System.Text;

    internal class DeleteSiteCriteria : CustomerClassCommandBaseCriteria
    {
        internal DeleteSiteCriteria(DeleteSiteSettingRequest request, int userId, string ip) : base(userId, ip)
        {
            PolicySites = ToXml(request);
        }

        public string PolicySites { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySiteDel";

        private static string ToXml(DeleteSiteSettingRequest request)
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in request.SiteSettings)
            {
                xmlBuilder
                    .Append("<r PolicyID=\"").Append(item.PolicyId).Append("\"")
                    .Append(" Site=\"").Append(item.Site).Append("\"")
                    .Append(" Currency=\"").Append(item.Currency).Append("\"")
                    .Append(" Platform=\"").Append(item.Platform).Append("\"/>");
            }

            xmlBuilder.Append("</ROOT>");
            return xmlBuilder.ToString();
        }
    }
    
}
