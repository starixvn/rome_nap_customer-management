namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal interface ISiteService
    {
        PagedSiteSetting GetPagedSiteSetting(GetSiteSettingRequest request, string sportTypes);

        IEnumerable<SiteSetting> GetDuplicatedSiteSettings(AssignSiteSettingRequest request);

        Result AssignSiteSetting(AssignSiteSettingRequest request, bool isOverwrite);

        Result Upload(IEnumerable<SiteSettingExcel> siteSettingExcels, bool isOverwrite, int userId, string ip);

        Result Delete(DeleteSiteSettingRequest request, int userId, string ip);
    }

    internal class SiteService : ISiteService
    {
        private readonly IFilterService filterService;
        private readonly ISiteDataAccess siteDataAccess;

        public SiteService(IFilterService filterService = null, ISiteDataAccess siteDataAccess = null)
        {
            this.filterService = filterService ?? new FilterService();
            this.siteDataAccess = siteDataAccess ?? new SiteDataAccess();
        }

        public PagedSiteSetting GetPagedSiteSetting(GetSiteSettingRequest request, string sportTypes)
        {
            var resultSet = siteDataAccess.GetPagedSiteSetting(new GetSiteSettingCriteria(request, sportTypes));

            return new PagedSiteSetting(resultSet.Item1, BuildSiteSettings(resultSet.Item2));
        }

        public IEnumerable<SiteSetting> GetDuplicatedSiteSettings(AssignSiteSettingRequest request)
        {
            var siteSettings = siteDataAccess.GetSiteSettings(new GetDuplicatedSiteSettingCriteria(request));

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
            => siteDataAccess.DoAction(new AssignSiteSettingCriteria(request, isOverwrite), Labels.AssignSiteSettingSuccessfully);

        public Result Upload(IEnumerable<SiteSettingExcel> siteSettingExcels, bool isOverwrite, int userId, string ip)
        {
            var criteria = new AssignSiteSettingCriteria(siteSettingExcels, isOverwrite, userId, ip);

            UploadRuleAsync(criteria).ConfigureAwait(false);

            return new Result(0, Labels.UploadSiteWarning);
        }

        public Result Delete(DeleteSiteSettingRequest request, int userId, string ip)
            => siteDataAccess.DoAction(new DeleteSiteCriteria(request, userId, ip), Labels.DeleteSiteSettingSuccessfully);

        private async Task UploadRuleAsync(AssignSiteSettingCriteria criteria)
        {
            var taskToRunSynchronously = Task.Run(() => siteDataAccess.ExecuteNonQuery(criteria, Labels.UploadExcelSuccessfully));

            await taskToRunSynchronously.ConfigureAwait(true);
        }
    }
}
