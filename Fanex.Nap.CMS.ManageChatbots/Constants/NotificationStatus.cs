using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Constants
{
    public static class NotificationStatus
    {
        public const byte Draft = 0;
        public const byte Initial = 1;
        public const byte Sending = 2;
        public const byte Completed = 3;
        public const byte Invalid = 4;
        public const byte Fail = 5;

        public static IDictionary<byte, string> NotificationStatusList
            => new Dictionary<byte, string>
            {
                [Draft] = nameof(Draft),
                [Initial] = nameof(Initial),
                [Sending] = nameof(Sending),
                [Completed] = nameof(Completed),
                [Invalid] = nameof(Invalid),
                [Fail] = nameof(Fail)
            };
    }
}
