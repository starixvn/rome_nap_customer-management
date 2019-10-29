namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;
    using Resources;

    public static class AccountStatusConst
    {
        public const byte Inactive = 1;
        public const byte Suspended = 2;
        public const byte ViewOnly = 3;
        public const byte Active = 4;

        public static IDictionary<byte, string> AccountStatusList
            => new Dictionary<byte, string>
            {
                [0] = ProblemAccountResource.All,
                [Active] = nameof(Active),
                [ViewOnly] = ProblemAccountResource.ViewOnly,
                [Suspended] = nameof(Suspended),
                [Inactive] = nameof(Inactive)
            };
    }
}