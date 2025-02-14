namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class GetAllFunctionsCriteria : CriteriaBase
    {
#pragma warning disable S2325
        public bool IsTest => AppSettings.IsTest;
#pragma warning restore S2325

        public override string GetSettingKey()
            => "Age_Chatbot_BotFunctionSelAll";

        public override bool IsValid()
            => true;
    }
}
