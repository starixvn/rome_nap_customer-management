namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Criteria.ChatbotMapping;
    using Repositories.ChatbotMapping;
    using Models.ChatbotMapping;
    using ViewModels.ChatbotMapping;

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

        public ChatBotsMappingModel SearchChatBots(SearchChatBotsViewModel paramModel)
        {
            var rawData = _repository.GetChatBots(new GetChatBotsCriteria(paramModel.MessengerName, paramModel.Username,
                paramModel.IsSubAccOnly, paramModel.Site,paramModel.LinkStatus, paramModel.Level, paramModel.Currency,paramModel.Status));

            return new ChatBotsMappingModel(rawData.Count(), rawData);
        }
    }
}