namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;
    using Infrastructure.Models;
    using Resources;

    public static class OrderByConst
    {
        public const byte OrderBySMALine = 1;
        public const byte OrderByMarkDate = 2;
        public const byte OrderByAccountStatus = 3;

        public static IEnumerable<IdNameModel> OrderByList()
        {
            yield return new IdNameModel { Id = OrderBySMALine, Name = ProblemAccountResource.SMALine };
            yield return new IdNameModel { Id = OrderByMarkDate, Name = ProblemAccountResource.MarkDate };
            yield return new IdNameModel { Id = OrderByAccountStatus, Name = ProblemAccountResource.AccountStatus };
        }
    }
}
