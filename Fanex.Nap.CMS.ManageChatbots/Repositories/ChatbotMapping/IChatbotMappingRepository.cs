namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotMapping
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using Fanex.Nap.CMS.ManageChatBots.Models.ChatbotMapping;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping;
    using Fanex.Nap.CMS.Infrastructure.Models;
    
    public interface IChatbotMappingRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria);
        
        IEnumerable<SiteModel> GetSites(ICriteria criteria);
        
        IEnumerable<IdNameModel> GetChatBotNames(ICriteria criteria);
        
        IEnumerable<NapAccountingPolicyModel> GetNapAccountingPolicy(ICriteria criteria);
        
        ChatBotsMappingModel GetChatBots(ICriteria criteria);
        
        ChatBotsConfigModel GetConfigChatBot(ICriteria criteria);
        
        Result ExecuteNonQuery(ICriteria criteria);

    }
}