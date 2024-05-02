namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class AssignSiteSettingCriteria : GetDuplicatedSiteSettingCriteria
    {
        public AssignSiteSettingCriteria(AssignSiteSettingRequest request, bool isOverwrite)
            : base(request)
        {
            IsOverwrite = isOverwrite;
            UserId = request.UserId;
            IP = request.IP;
        }

        public AssignSiteSettingCriteria(
            IEnumerable<SiteSettingExcel> siteSettingExcels,
            bool isOverwride,
            int userId,
            string ip)
        {
            PolicySites = ToXml(siteSettingExcels);
            IsOverwrite = isOverwride;
            UserId = userId;
            IP = ip;
        }

        public bool IsOverwrite { get; }

        public int UserId { get; }

        public string IP { get; }

        public byte ActionID { get; set; }
        

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySiteIns_Loop";

        private static string ToXml(IEnumerable<SiteSettingExcel> siteSettingExcels)
        {
            var xmlBuilder = new StringBuilder();

            foreach (var item in siteSettingExcels)
            {
                AppendElement(
                    xmlBuilder,
                    item.PolicyId,
                    string.Join(",", item.Sites.Distinct()),
                    string.Join(",", item.Currencies.Distinct()),
                    string.Join(",", item.Platforms.Distinct()));
            }

            return InsertRoot(xmlBuilder.ToString());
        }
    }
}
