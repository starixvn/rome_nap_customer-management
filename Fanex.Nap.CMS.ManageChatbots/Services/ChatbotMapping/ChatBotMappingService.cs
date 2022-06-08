using System;
using System.Threading.Tasks;
using Fanex.Nap.CMS.Infrastructure.Configurations;

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
    using TelegramBot.Client.Services;
    using System.Net.Http;
    
    public class ChatBotMappingService : IChatBotMappingService
    {
        private readonly IChatbotMappingRepository _repository;
        private readonly NotificationService _notificationService;

        public ChatBotMappingService(IChatbotMappingRepository repository = null)
        {
            var telegramClient = HttpClientFactory.Create();
            telegramClient.BaseAddress = AppSettings.TelegramBotClientUri;
            _notificationService =  new NotificationService(telegramClient);
            
            _repository = repository ?? new ChatBotMappingRepository();
        }

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            return _repository.GetCurrencies(new GetCurrencyListCriteria { Mode = 1 });
        }

        public IEnumerable<IdNameModel> GetSites()
        {
            return _repository.GetSites(new GetSiteCriteria { Mode = 0 });
        }

        public ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel)
        {
            var criteria = new GetConfigChatBotCriteria
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
            return rawData;
        }

        public Result UpdateFunctionsConfig(ChatBotFunctionConfig[] functionConfigs, int creatorID, string ip,
            int userID, int botID)
        {
            var criteria = new UpdFunctionsChatBotCriteria
            {
                Creator = creatorID,
                IP = ip,
                FunctionConfigs = functionConfigs,
                UserID = userID,
                BotID=botID
            };

            var result = _repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Functions");
            }

            return result;
        }

        public Result UpdateLanguageConfig(int creatorID, string ip, int userID, int botID, string language)
        {
            var criteria = new UpdLanguageChatBotCriteria
            {
                Creator = creatorID,
                IP = ip,
                Language = language,
                UserID = userID,
                BotID = botID
            };

            var result = _repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Language");
            }

            return result;
        }

        public Result UnlinkChatBot(int creatorID, string ip, int userId, int botId, int subAccId)
        {
            var criteria = new UnlinkChatBotCriteria
            {
                Creator = creatorID,
                IP = ip,
                Status = false,
                IsVerifyOTP = false,
                UserID = userId,
                BotID = botId,
            };
            try
            {
                _notificationService.Unlink(userId, subAccId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
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