namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class SiteViewModel : BaseViewModel
    {
        public const int UploadLimitRows = 10_000;

        public SiteViewModel(
            bool hasHostPermission,
            IEnumerable<IdNameModel> sites,
            IEnumerable<IdNameModel> policyTitles,
            IEnumerable<CurrencyModel> currencies,
            IEnumerable<IdNameModel> sportTypes) : base(hasHostPermission)
        {
            Sites = sites;
            PolicyTitles = policyTitles.Select(p => ToSelectListItem(p)).InsertAllItem();
            Currencies = currencies.Select(c => new IdNameModel { Id = c.ExchangeId, Name = c.Currency }); ;
            SportTypes = sportTypes.Select(c => ToSelectListItem(c));
        }

        public IEnumerable<IdNameModel> Sites { get; }

        public IEnumerable<SelectListItem> PolicyTitles { get; }

        public IEnumerable<IdNameModel> Platforms { get; }
            = PlatformConst.Platforms.Where(pl => pl.Key != 0).Select(kvp => new IdNameModel { Id = kvp.Key, Name = kvp.Value }).ToList();

        public IEnumerable<IdNameModel> Currencies { get; }

        public IEnumerable<SelectListItem> SportTypes { get; }
    }
}
