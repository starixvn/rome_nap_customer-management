namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;

    public static class CurrencyConst
    {
        public const int CY = 81;
        public const int UUS = 20;

        public static IDictionary<int, string> CurrencyList
            => new Dictionary<int, string>
            {
                [CY] = nameof(CY),
                [UUS] = nameof(UUS),
            };
    }
}
