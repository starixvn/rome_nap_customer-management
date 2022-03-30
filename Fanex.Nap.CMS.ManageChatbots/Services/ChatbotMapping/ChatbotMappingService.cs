using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping;
using Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotMapping;
using System.Linq;

namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatbotMapping
{
    public class ChatbotMappingService:IChatbotMappingService
    {
        private const string CurrencyCacheKey = "Fanex.Nap.CMS.ManageChatbots.Services.ChatbotMapping.Currency";
        
        private readonly IChatbotMappingRepository chatbotMappingRepository;
        
        public IEnumerable<IdNameModel> GetCurrencies()
        {
            var currencies = CacheManager.Get<List<IdNameModel>>(CurrencyCacheKey);

            if (currencies == null)
            {
                currencies = chatbotMappingRepository
                    .GetCurrencies(new CurrencyListCriteria())
                    .ToList();

                if (currencies.Count > 0)
                {
                    currencies.Insert(0, new IdNameModel { Id = 0, Name = Infrastructure.Resources.Labels.All });
                    CacheManager.Set(CurrencyCacheKey, currencies);
                }
            }

            return currencies;
        }
    }
}