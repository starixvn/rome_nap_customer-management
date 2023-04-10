namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;

    internal interface ISiteService
    {
        PagedSiteSetting GetPagedSiteSetting(GetSiteSettingRequest request);

        IEnumerable<SiteSetting> GetDuplicatedSiteSettings(AssignSiteSettingRequest request);

        Result AssignSiteSetting(AssignSiteSettingRequest request);

        Result Upload(IEnumerable<SiteSettingExcel> siteSettingExcels, int userId, string ip);
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

        public PagedSiteSetting GetPagedSiteSetting(GetSiteSettingRequest request)
        {
            var resultSet = siteDataAccess.GetPagedSiteSetting(new GetSiteSettingCriteria(request));

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
                item.IsAllCurrencies = allCurrencyCount == NumberOfItemsSeparatedByComma(item.Currencies);
                item.PlatformNames = string.Join(", ", item.Platforms
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => platformDict[Convert.ToByte(p)]));

                yield return item;
            }
        }

        private static int NumberOfItemsSeparatedByComma(string value)
            => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

        public Result AssignSiteSetting(AssignSiteSettingRequest request)
            => siteDataAccess.DoAction(new AssignSiteSettingCriteria(request), Labels.AssignSiteSettingSuccessfully);

        public Result Upload(IEnumerable<SiteSettingExcel> siteSettingExcels, int userId, string ip)
            => siteDataAccess.DoAction(
                new AssignSiteSettingCriteria(siteSettingExcels, userId, ip),
                Labels.UploadExcelSuccessfully);
    }
}
