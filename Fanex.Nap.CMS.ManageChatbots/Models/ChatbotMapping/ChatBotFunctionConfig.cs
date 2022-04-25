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
        public ChatBotFunctionConfig(int functionId, bool isEnabled)
        {
            FunctionId = functionId;
            IsEnabled = isEnabled;
        }
        public ChatBotFunctionConfig()
        {
            
        }
        public int FunctionId { get; set; }
        
        public string FunctionName { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}