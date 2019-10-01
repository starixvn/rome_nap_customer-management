namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;
    using Resources;

    public static class IsReducedConst
    {
        public const byte No = 1;
        public const byte Yes = 2;

        public static IDictionary<byte, string> IsReducedList
            => new Dictionary<byte, string>
            {
                [0] = ProblemAccountResource.All,
                [Yes] = nameof(Yes),
                [No] = nameof(No)
            };
    }
}
