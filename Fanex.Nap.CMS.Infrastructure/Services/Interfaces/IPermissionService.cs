namespace Fanex.Nap.CMS.Infrastructure.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IPermissionService
    {
        bool HasSpecialSitePermission(int userId);

        IEnumerable<NAPAccPolicy> GetNAPAccPolicies(int userId);
    }
}
