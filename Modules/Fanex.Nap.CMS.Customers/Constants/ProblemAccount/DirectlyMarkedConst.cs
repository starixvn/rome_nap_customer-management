namespace Fanex.Nap.CMS.Customers.Constants.ProblemAccount
{
    using System.Collections.Generic;
    using Infrastructure.Models;
    using Resources;

    public static class DirectlyMarkedConst
    {
        public const byte No = 1;
        public const byte Yes = 2;

        public static IEnumerable<IdNameModel> DirectlyMarkedList()
        {
            yield return new IdNameModel { Id = 0, Name = ProblemAccountResource.All };
            yield return new IdNameModel { Id = Yes, Name = nameof(Yes) };
            yield return new IdNameModel { Id = No, Name = nameof(No) };
        }
    }
}
