namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.Core.Business.Constant;

    public class SportbookBaseController : BaseController
    {
        private const string SportbookPrefixFunctionName = "SportsbookCCManagement_";

        public IEnumerable<IdNameModel> GetHavePermissionSportTypes(IEnumerable<IdNameModel> sportTypes, Policy policy)
        {
            return sportTypes.Where(sportType => HasRight(string.Concat(SportbookPrefixFunctionName, sportType.Id), policy));
        }

        public bool HavePermissionOnSportType(short sportType, Policy policy)
        {
            return HasRight(string.Concat(SportbookPrefixFunctionName, sportType), policy);
        }

        public bool HavePermissionOnSportType(IEnumerable<short> sportTypes, Policy policy)
        {
            foreach (var sportType in sportTypes)
            {
                if(!HasRight(string.Concat(SportbookPrefixFunctionName, sportType), policy))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
