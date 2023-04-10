namespace Fanex.Nap.CMS.CustomerClass.Constants
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.CustomerClass.Resources;

    internal static class IndicatorConst
    {
        public static IDictionary<byte, string> Indicators
            => new Dictionary<byte, string>
            {
#pragma warning disable S109
                [2] = Labels.All,
#pragma warning restore S109
                [1] = Labels.Yes,
                [0] = Labels.No
            };
    }
}
