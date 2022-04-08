namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Criteria.ChatbotMapping;
    using Repositories.ChatbotMapping;
    using Models.ChatbotMapping;

    public class ChatBotMappingService : IChatBotMappingService
    {
        private readonly IChatbotMappingRepository _repository;

        public ChatBotMappingService(IChatbotMappingRepository repository = null)
        {
            _repository = repository ?? new ChatBotMappingRepository();
        }

        public IEnumerable<SelectListItem> GetCurrencies()
        {
            return _repository.GetCurrencies(new CurrencyListCriteria());
        }

        public IEnumerable<SelectListItem> GetSites(bool hasSpecialSitePermission)
        {
            return _repository.GetSites(new GetCreditSiteCriteria(hasSpecialSitePermission));
        }

        public bool HasSpecialSitePermission(int customerId)
        {
            var accountingPolicy = _repository.GetNapAccountingPolicy(new GetNapAccountingPolicy(customerId));
            var isHasAccessSpecialSites = accountingPolicy.Any(policy => policy.FunctionName == "[AccessSpecialSites]");

            return isHasAccessSpecialSites;
        }

        public ChatBotsMappingModel SearchChatBots()
        {
            return new ChatBotsMappingModel(2, new ChatBotModel[]
            {
                new ChatBotModel("botA", "JackMessenger", "subaccount", "linkStatus", "eng", "Supper", "VND", "live"),
                new ChatBotModel("botB", "JackMessenger2", "subaccount", "linkStatus", "eng", "Supper", "VND", "live")
            });
            
            _repository.GetChatBots()
        }
    }
}