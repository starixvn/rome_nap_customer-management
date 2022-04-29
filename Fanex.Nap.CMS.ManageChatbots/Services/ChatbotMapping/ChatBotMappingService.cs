namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.ChatbotMapping;
    using Repositories.ChatbotMapping;
    using Models.ChatbotMapping;
    using ViewModels.ChatbotMapping;
    using Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Resources;

    public class ChatBotMappingService : IChatBotMappingService
    {
        private readonly IChatbotMappingRepository _repository;

        public ChatBotMappingService(IChatbotMappingRepository repository = null)
        {
            _repository = repository ?? new ChatBotMappingRepository();
        }

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            return _repository.GetCurrencies(new GetCurrencyListCriteria());
            ;
        }

        public IEnumerable<IdNameModel> GetSites()
        {
            return _repository.GetSites(new GetSiteCriteria { Mode = 0 });
        }

        public ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel)
        {
            var criteria = new GetConfigChatBotCriteria()
            {
                CustID = paramModel.CusId,
                BotID = paramModel.BotId,
                SubAccID = paramModel.SubAccId
            };

            var chatBotsConfig = _repository.GetConfigChatBot(criteria);
            return chatBotsConfig;
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

        public Result UpdateFunctionsConfig(ChatBotFunctionConfig[] functionConfigs, int CreatorID, string IP,
            int userID)
        {
            var criteria = new UpdFunctionsChatBotCriteria()
            {
                Creator = CreatorID,
                IP = IP,
                FunctionConfigs = functionConfigs,
                UserID = userID
            };

            var result = _repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Functions");
            }

            return result;
        }

        public Result UpdateLanguageConfig(int CreatorID, string IP, int UserID, int BotID, string Language)
        {
            var criteria = new UpdLanguageChatBotCriteria()
            {
                Creator = CreatorID,
                IP = IP,
                Language = Language,
                UserID = UserID,
                BotID = BotID
            };

            var result = _repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Language");
            }

            return result;
        }

        public Result UnlinkChatBot(int CreatorID, string IP, int UserID, int BotID, bool status, bool IsVerifyOTP)
        {
            var criteria = new UnlinkChatBotCriteria()
            {
                Creator = CreatorID,
                IP = IP,
                Status = status,
                IsVerifyOTP = IsVerifyOTP,
                UserID = UserID,
                BotID = BotID
            };

            var result = _repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Status");
            }

            return result;
        }

        public bool HasSpecialSitePermission(int customerId)
        {
            var accountingPolicy =
                _repository.GetNapAccountingPolicy(new GetNapAccountingPolicy { CustID = customerId });
            var isHasAccessSpecialSites =
                accountingPolicy.Any(policy => policy.FunctionName == CMSFunctionKeys.SpecialSites);

            return isHasAccessSpecialSites;
        }
    }
}