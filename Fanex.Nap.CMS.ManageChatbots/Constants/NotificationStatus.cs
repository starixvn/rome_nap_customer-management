
namespace Fanex.Nap.CMS.ManageChatbots.Constants
{
    using System.Collections.Generic;

    public static class NotificationStatus
    {
        public const byte All = 0;
        public const byte Draft = 1;
        public const byte Initial = 2;
        public const byte Sending = 3;
        public const byte Completed = 4;
        public const byte Invalid = 5;
        public const byte Fail = 6;

        public static IDictionary<byte, string> NotificationStatusList
            => new Dictionary<byte, string>
            {
                [All] = nameof(All),
                [Draft] = nameof(Draft),
                [Initial] = nameof(Initial),
                [Sending] = nameof(Sending),
                [Completed] = nameof(Completed),
                [Invalid] = nameof(Invalid),
                [Fail] = nameof(Fail)
            };
    }
}
