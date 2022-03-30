namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotMapping
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;
    
    public interface IChatbotMappingRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria);
    }
}