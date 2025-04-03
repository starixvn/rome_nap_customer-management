namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;

    internal interface ISiteService
    {
        PagedSiteSetting GetPagedSiteSetting(GetSiteSettingRequest request, string sportTypes);

        IEnumerable<SiteSetting> GetDuplicatedSiteSettings(AssignSiteSettingRequest request);

        Result AssignSiteSetting(AssignSiteSettingRequest request, bool isOverwrite);

        Result Upload(IEnumerable<SiteSettingExcel> siteSettingExcels, bool isOverwrite, int userId, string ip);

        Result Delete(DeleteSiteSettingRequest request, int userId, string ip);

        ProgressResponse  GetUploadProgress(int userId);
    }

    internal class SiteService : ServiceBase, ISiteService
    {
        private readonly IFilterService filterService;

        private const string LastUpdatedTime = "Fanex.CMS.CCM.Site.LastUpdatedTime-{0}";

        public SiteService() : this(new FilterService())
        {
        }

        public SiteService(IFilterService filterService) : base()
        {
            this.filterService = filterService ?? new FilterService();
        }

        public PagedSiteSetting GetPagedSiteSetting(GetSiteSettingRequest request, string sportTypes)
        {
            var criteria = new GetSiteSettingCriteria
            {
                Site = request.Sites ?? string.Empty,
                PolicyId = request.PolicyId,
                Platform = request.Platforms ?? string.Empty,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SportTypes = sportTypes
            };
            var siteSettings = DynamicRepository.Fetch<SiteSetting>(criteria);

            return new PagedSiteSetting(criteria.TotalItems, BuildSiteSettings(siteSettings));
        }

        public IEnumerable<SiteSetting> GetDuplicatedSiteSettings(AssignSiteSettingRequest request)
        {
            var criteria = new GetDuplicatedSiteSettingCriteria(ToDuplicatedSiteSettingXml(request));

            var siteSettings = DynamicRepository.Fetch<SiteSetting>(criteria);

            return BuildSiteSettings(siteSettings);
        }

        private IEnumerable<SiteSetting> BuildSiteSettings(IEnumerable<SiteSetting> siteSettings)
        {
            var allCurrencyCount = filterService.GetAllCurrencies().Count();
            var platformDict = PlatformConst.Platforms;

            foreach (var item in siteSettings)
            {
                item.IsAllCurrencies = allCurrencyCount == NumberOfItemsSeparatedByComma(item.Currencies) || item.Currencies.ToLowerInvariant() == "all";
                item.PlatformNames = string.Join(", ", item.Platforms
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => platformDict[Convert.ToByte(p)]));

                yield return item;
            }
        }

        private static int NumberOfItemsSeparatedByComma(string value)
            => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

        public Result AssignSiteSetting(AssignSiteSettingRequest request, bool isOverwrite)
        {
            var criteria = new AssignSiteSettingCriteria(ToDuplicatedSiteSettingXml(request), isOverwrite)
            {
                UserId = request.UserId,
                IP = request.IP
            };

            return DoAction(criteria, Labels.AssignSiteSettingSuccessfully);
        }

        public Result Upload(IEnumerable<SiteSettingExcel> siteSettingExcels, bool isOverwrite, int userId, string ip)
        {
            var criteria = new AssignSiteSettingCriteria(ToAssignSiteSettingXml(siteSettingExcels), isOverwrite, userId, ip) { ActionID = 1 };

            return DoAction(criteria, Labels.UploadSiteWarning);
        }

        public Result Delete(DeleteSiteSettingRequest request, int userId, string ip)
        {
            var criteria = new DeleteSiteCriteria
            {
                PolicySites = ToDeleteSiteXml(request),
                UserId = userId,
                IP = ip
            };

            return DoAction(criteria, Labels.DeleteSiteSettingSuccessfully);
        }

        public ProgressResponse GetUploadProgress(int userId)
        {
            var lastUpdatedTimeKey = string.Format(LastUpdatedTime, userId);
            var criteria = new GetSiteUploadProgressCriteria() { UserID = userId, LastModifiedTime = CacheManager.Get<DateTime?>(lastUpdatedTimeKey) };

            DynamicRepository.Execute(criteria);
            if (criteria.CountBatch > 0 && criteria.InitialBatch > 0)
            {
                if (criteria.LastModifiedTime == null)
                {
                    var lastUpdatedTime = criteria.LastModifiedTime;
                    CacheManager.Set(lastUpdatedTimeKey, lastUpdatedTime, new TimeSpan(1, 0, 0, 0));
                }
            }
            else
            {
                CacheManager.Remove(lastUpdatedTimeKey);
            }

            return new ProgressResponse(criteria.CountBatch, criteria.InitialBatch);
        }
        private static string ToDeleteSiteXml(DeleteSiteSettingRequest request)
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

        private static string ToAssignSiteSettingXml(IEnumerable<SiteSettingExcel> siteSettingExcels)
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

        private static string ToDuplicatedSiteSettingXml(AssignSiteSettingRequest request)
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
