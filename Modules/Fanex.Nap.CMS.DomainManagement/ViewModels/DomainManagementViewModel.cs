namespace Fanex.Nap.CMS.DomainManagement.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.DomainManagement.Constants;
    using Fanex.Nap.CMS.DomainManagement.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using CommonLabels = Infrastructure.Resources.Labels;

    public class DomainManagementViewModel
    {
        public DomainManagementViewModel(
            IEnumerable<Tuple<string, IEnumerable<IdNameModel>>> sites,
            IEnumerable<Country> countries,
            bool hasHostPermission)
        {
            Sites = BuildSites(sites);
            Status = DomainStatusConst.DomainStatus.Select(d => new SelectListItem { Text = d.Value, Value = d.Key.ToString() }).ToList();
            Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Code });
            HasHostPermission = hasHostPermission;
        }

        public IEnumerable<SelectListItem> Systems { get; } = new[]
        {
            new SelectListItem { Text = Labels.Agency, Value = Labels.Agency },
            new SelectListItem { Text = CommonLabels.Member, Value = CommonLabels.Member }
        };

        public IEnumerable<SelectListItem> Sites { get; }

        public IEnumerable<SelectListItem> Status { get; }

        public IEnumerable<SelectListItem> Countries { get; }

        public IEnumerable<SelectListItem> BrandNames { get; } = new[]
        {
            new SelectListItem { Text = "B", Value = "B" },
            new SelectListItem { Text = "V", Value = "V" },
        };

        public IEnumerable<SelectListItem> OfferTypes { get; } = new[]
        {
            new SelectListItem { Text = Labels.Yes, Value = "1" },
            new SelectListItem { Text = Labels.No, Value = "0" },
        };

        public bool HasHostPermission { get; }

        private static IEnumerable<SelectListItem> BuildSites(IEnumerable<Tuple<string, IEnumerable<IdNameModel>>> siteModels)
        {
            foreach (var item in siteModels)
            {
                var group = new SelectListGroup { Name = item.Item1 };

                foreach (var site in item.Item2)
                {
                    yield return new SelectListItem
                    {
                        Text = site.Name,
                        Value = site.Id.ToString(),
                        Group = group
                    };
                }
            }
        }
    }
}
