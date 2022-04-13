namespace Fanex.Nap.CMS.ManageChatBots.Models.ChatbotMapping
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