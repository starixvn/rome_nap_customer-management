namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Customers.Resources;

    public static class DirectlyMarkedConst
    {
        public const byte All = 0;
        public const byte No = 1;
        public const byte Yes = 2;

        public static IDictionary<byte, string> DirectlyMarkedList
            => new Dictionary<byte, string>
            {
                [All] = ProblemAccountResource.All,
                [Yes] = nameof(Yes),
                [No] = nameof(No)
            };
    }
}
