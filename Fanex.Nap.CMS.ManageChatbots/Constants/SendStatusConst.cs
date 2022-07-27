namespace Fanex.Nap.CMS.ManageChatbots.Constants
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageChatbots.Resources;

    public static class SendStatusConst
    {
        public const byte All = 0;
        public const byte Sent = 1;
        public const byte Failed = 2;
        public const byte Duplicated = 3;
        public const byte BotBlocked = 4;

        public static IDictionary<byte, string> SendStatusList
            => new Dictionary<byte, string>
            {
                [All] = nameof(All),
                [Failed] = nameof(Failed),
                [Sent] = nameof(Sent),
                [Duplicated] = nameof(Duplicated),
                [BotBlocked] = Labels.BotBlocked
            };
    }
}
