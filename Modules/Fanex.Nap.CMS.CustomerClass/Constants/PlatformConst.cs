namespace Fanex.Nap.CMS.CustomerClass.Constants
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.CustomerClass.Resources;

    internal static class PlatformConst
    {
        public static IDictionary<byte, string> Platforms
            => new Dictionary<byte, string>
            {
                [1] = Labels.Desktop,
#pragma warning disable S109
                [2] = Labels.Mobile,
                [3] = Labels.App,
                [4] = Labels.Oddsfeed,
                [5] = Labels.DirectAPI
#pragma warning restore S109
            };
    }
}
