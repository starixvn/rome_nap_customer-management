namespace Fanex.Nap.CMS.ManageChatBots.Models.ChatBotMapping
{
    public class NapAccountingPolicyModel
    {
        public NapAccountingPolicyModel(
            string functionName
        )
        {
            FunctionName = functionName;
        }

        public string FunctionName { get; }
    }
}