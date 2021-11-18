namespace Fanex.Nap.CMS.Customers.Constants.LimitDailyPTUpdate
{
    using System.Collections.Generic;
    using Resources;

    public static class TypeConst
    {
        public const byte All = 0;
        public const byte SMALine = 1;
        public const byte Site = 2;
        public const byte Currency = 3;
        public const byte SiteCurrency = 4;

        public static IDictionary<byte, string> TypeList
            => new Dictionary<byte, string>
            {
                [All] = nameof(All),
                [SMALine] = LimitDailyPTUpdateResource.SMALine,
                [Site] = nameof(Site),
                [Currency] = nameof(Currency),
                [SiteCurrency] = LimitDailyPTUpdateResource.SiteCurrency
            };
    }
}
