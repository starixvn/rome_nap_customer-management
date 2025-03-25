namespace Fanex.Nap.CMS.ManageChatbots.Criteria.Share
{
    using Fanex.Data.Repository;

    public class GetChatbotNamesCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_chatbot_BotSelAll";

        public override bool IsValid()
            => true;
    }
}