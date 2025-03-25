namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            var criteria = new GetSiteSettingCriteria(request, sportTypes);
            var siteSettings = DynamicRepository.Fetch<SiteSetting>(criteria);

            return new PagedSiteSetting(criteria.TotalItems, BuildSiteSettings(siteSettings));
        }

        public IEnumerable<SiteSetting> GetDuplicatedSiteSettings(AssignSiteSettingRequest request)
        {
            var siteSettings = DynamicRepository.Fetch<SiteSetting>(new GetDuplicatedSiteSettingCriteria(request));

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
            => DoAction(new AssignSiteSettingCriteria(request, isOverwrite), Labels.AssignSiteSettingSuccessfully);

        public Result Upload(IEnumerable<SiteSettingExcel> siteSettingExcels, bool isOverwrite, int userId, string ip)
        {
            var criteria = new AssignSiteSettingCriteria(siteSettingExcels, isOverwrite, userId, ip) { ActionID = 1 };

            return DoAction(criteria, Labels.UploadSiteWarning);
        }

        public Result Delete(DeleteSiteSettingRequest request, int userId, string ip)
            => DoAction(new DeleteSiteCriteria(request, userId, ip), Labels.DeleteSiteSettingSuccessfully);

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
    }
}
