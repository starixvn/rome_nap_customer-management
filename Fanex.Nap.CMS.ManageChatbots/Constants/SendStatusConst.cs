using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Constants
{
    public static class SendStatusConst
    {
        public const byte All = 0;
        public const byte Sent = 1;
        public const byte Failed = 2;

        public static IDictionary<byte, string> SendStatusList
            => new Dictionary<byte, string>
            {
                [All] = nameof(All),
                [Failed] = nameof(Failed),
                [Sent] = nameof(Sent),
            };
    }
}
