namespace Fanex.Nap.CMS.Customers.Constants.ProblemAccount
{
    using System.Collections.Generic;
    using Infrastructure.Models;

    public static class CurrencyConst
    {
        public const int CY = 81;
        public const int UUS = 20;

        public static IEnumerable<IdNameModel> CurrencyList()
        {
            yield return new IdNameModel { Id = CY, Name = nameof(CY) };
            yield return new IdNameModel { Id = UUS, Name = nameof(UUS) };
        }
    }
}
