using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatbotMapping
{
    public interface IChatbotMappingService
    {
        IEnumerable<IdNameModel> GetCurrencies();
    }
}