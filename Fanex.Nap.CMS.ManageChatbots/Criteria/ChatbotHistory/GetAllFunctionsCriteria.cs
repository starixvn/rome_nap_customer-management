namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class GetAllFunctionsCriteria : CriteriaBase
    {
        public bool IsTest => AppSettings.IsTest;

        public override string GetSettingKey()
            => "Age_Chatbot_BotFunctionSelAll";
    }
}
