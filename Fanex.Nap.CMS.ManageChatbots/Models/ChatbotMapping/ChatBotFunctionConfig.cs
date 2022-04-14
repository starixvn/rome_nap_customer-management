namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping
{
    public class ChatBotFunctionConfig
    {
        public ChatBotFunctionConfig(int functionId, string functionName, bool isEnabled)
        {
            FunctionId = functionId;
            FunctionName = functionName;
            IsEnabled = isEnabled;
        }

        public int FunctionId { get; set; }
        
        public string FunctionName { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}