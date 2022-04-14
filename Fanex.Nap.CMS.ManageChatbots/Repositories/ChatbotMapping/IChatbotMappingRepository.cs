namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.Framework.Data;
    using Fanex.Nap.CMS.ManageChatBots.Models.ChatbotMapping;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping;

    public interface IChatbotMappingRepository
    {
        IEnumerable<SelectListItem> GetCurrencies(ICriteria criteria);
        IEnumerable<SelectListItem> GetSites(ICriteria criteria);
        IEnumerable<NapAccountingPolicyModel> GetNapAccountingPolicy(ICriteria criteria);
        IEnumerable<ChatBotModel> GetChatBots(ICriteria criteria);
        ChatBotsConfigModel GetConfigChatBot(ICriteria criteria);
    }
}