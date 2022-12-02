namespace Fanex.Nap.CMS.ManageChatbots.Constants
{
    public static class HiddenNotifyFunction
    {
        public const int ChangePasswordAlert = 9;
        
        public const int ChangeSecurityCodeAlert = 10;

        public const int WrongPasswordAlert = 11;

        public const int OTP = 15;

        public static int[] HiddenNotifyFunctions => new[] { ChangePasswordAlert, ChangeSecurityCodeAlert, WrongPasswordAlert, OTP };
    }
}
