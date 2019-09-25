namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;
    using Resources;

    public static class OrderByConst
    {
        public const byte OrderBySMALine = 1;
        public const byte OrderByMarkDate = 2;
        public const byte OrderByAccountStatus = 3;

        public static IDictionary<byte, string> OrderByList
            => new Dictionary<byte, string>
            {
                [OrderBySMALine] = ProblemAccountResource.SMALine,
                [OrderByMarkDate] = ProblemAccountResource.MarkDate,
                [OrderByAccountStatus] = ProblemAccountResource.AccountStatus
            };
    }
}
