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
            return _repository.GetCurrencies(new GetCurrencyListCriteria());
        }

        public IEnumerable<SelectListItem> GetSites(bool hasSpecialSitePermission)
        {
            return _repository.GetSites(new GetSiteCriteria(0));
        }

        public bool HasSpecialSitePermission(int customerId)
        {
            var accountingPolicy = _repository.GetNapAccountingPolicy(new GetNapAccountingPolicy(customerId));
            var isHasAccessSpecialSites = accountingPolicy.Any(policy => policy.FunctionName == "[AccessSpecialSites]");

            return isHasAccessSpecialSites;
        }

        public ChatBotsMappingModel SearchChatBots(SearchChatBotsViewModel paramModel, int customerId)
        {
            var criteria = new GetChatBotsCriteria
            {
                ChatID = paramModel.ChatID,
                Username = paramModel.Username,
                IsSubAccOnly = paramModel.IsSubAccOnly,
                Site = paramModel.Site,
                LinkStatus = paramModel.LinkStatus,
                Level = paramModel.Level,
                Currency = paramModel.Currency,
                Status = paramModel.AccStatus,
                PageIndex = paramModel.PageIndex + 1,
                PageSize = paramModel.PageSize,
                IsSpecialSite = HasSpecialSitePermission(customerId)
            };
            
            
            var rawData = _repository.GetChatBots(criteria);

            return new ChatBotsMappingModel(rawData.Count(), rawData);
        }

        public ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel)
        {
            var chatBotsConfig =
                _repository.GetConfigChatBot(new GetConfigChatBotCriteria(paramModel.CusId, paramModel.BotId,
                    paramModel.SubAccId));

            return chatBotsConfig;
        }
    }
}