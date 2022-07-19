using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.ManageChatbots.Constants
{
    public static class HiddenNotifyFunction
    {
        public const int ChangePasswordAlert = 9;
        
        public const int ChangeSecurityCodeAlert = 10;

        public static int[] HiddenNotifyFunctions => new[] { ChangePasswordAlert, ChangeSecurityCodeAlert };
    }
}
