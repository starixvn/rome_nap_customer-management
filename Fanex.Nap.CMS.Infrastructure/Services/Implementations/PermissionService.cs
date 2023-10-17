namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;

    public class PermissionService : IPermissionService
    {
        private readonly IDynamicRepository dynamicRepository;

        public PermissionService(IDynamicRepository dynamicRepository = null)
            => this.dynamicRepository = dynamicRepository ?? new DynamicRepository();

        public bool HasSpecialSitePermission(int userId)
            => GetNAPAccPolicies(userId).Any(p => p.FuncName.Equals(CMSFunctionKeys.SpecialSites, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<NAPAccPolicy> GetNAPAccPolicies(int userId)
            => dynamicRepository.Fetch<NAPAccPolicy>(new GetNAPAccPolicyCriteria(userId));
    }
}
