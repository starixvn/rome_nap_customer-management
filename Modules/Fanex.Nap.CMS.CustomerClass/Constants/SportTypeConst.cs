namespace Fanex.Nap.CMS.CustomerClass.Constants
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.CustomerClass.Resources;

    internal static class SportTypeConst
    {
        public static IDictionary<short, string> SportTypes
            => new Dictionary<short, string>
            {
                [1] = Labels.Soccer,
#pragma warning disable S109
                [2] = Labels.Basketball,
                [99] = Labels.OtherSports,
                [43] = Labels.ESports
#pragma warning restore S109
            };
    }
}
