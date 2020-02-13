namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;
    using Resources;

    public static class AccountStatusCreditSiteConst
    {
        public const byte Open = 1;
        public const byte Suspended = 2;
        public const byte Closed = 3;
        public const byte Disabled = 4;

        public static IDictionary<byte, string> AccountStatusListForFilter
            => new Dictionary<byte, string>
            {
                [0] = ProblemAccountResource.All,
                [Open] = nameof(Open),
                [Closed] = nameof(Closed),
                [Suspended] = nameof(Suspended)
            };

        public static IDictionary<byte, string> AccountStatusList
            => new Dictionary<byte, string>
            {
                [Open] = nameof(Open),
                [Closed] = nameof(Closed),
                [Suspended] = nameof(Suspended),
                [Disabled] = nameof(Disabled)
            };
    }
}
