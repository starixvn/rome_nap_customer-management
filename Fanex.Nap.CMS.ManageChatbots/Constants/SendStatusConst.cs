using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Constants
{
    public static class SendStatusConst
    {
        public const byte Failed = 0;
        public const byte Sent = 1;

        public static IDictionary<byte, string> SendStatusList
            => new Dictionary<byte, string>
            {
                [Failed] = nameof(Failed),
                [Sent] = nameof(Sent)
            };
    }
}
