namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;
    using Infrastructure.Models;
    using Resources;

    public static class VIPConst
    {
        public const int Yes = 1;
        public const int No = 2;

        public static IEnumerable<IdNameModel> VIPList()
        {
            yield return new IdNameModel { Id = 0, Name = ProblemAccountResource.All };
            yield return new IdNameModel { Id = Yes, Name = nameof(Yes) };
            yield return new IdNameModel { Id = No, Name = nameof(No) };
        }
    }

}
