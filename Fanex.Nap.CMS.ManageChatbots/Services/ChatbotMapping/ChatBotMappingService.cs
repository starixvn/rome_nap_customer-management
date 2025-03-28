namespace Fanex.Nap.CMS.ManageChatbots.Services.ChatBotMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Criteria.ChatbotMapping;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Infrastructure;
    using Models.ChatbotMapping;
    using Resources;
    using TelegramBot.Client.Services;
    using ViewModels.ChatbotMapping;
    using Criteria.Share;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ManageChatBots.Models.ChatbotMapping;

    public class ChatBotMappingService : IChatBotMappingService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ChatBotMappingService() : this(new DynamicRepository()) { }

        public ChatBotMappingService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            var currencies = dynamicRepository.Fetch<CurrencyModel>(new GetCurrencyListCriteria { Mode = 1 });

            return currencies.Select(cur => new IdNameModel { Id = cur.ExchangeId, Name = cur.Currency });
        }

        public IEnumerable<SiteViewModel> GetSites()
        {
            var sites = new List<SiteViewModel>();
            dynamicRepository.Fetch<SiteGroup>(new GetSiteCriteria { Mode = 0 })
               .GroupBy(p => p.SiteLabel)
               .ToList()
               .ForEach(grp =>
               {
                   if (string.IsNullOrEmpty(grp.Key))
                   {
                       sites.AddRange(grp.Select(pt => new SiteViewModel(pt.Id.ToString(), pt.Name, null)));
                   }
                   else
                   {
                       sites.Add(new SiteViewModel(grp.Key, grp.Key, grp.Select(p => new SiteViewModel(p.Id.ToString(), p.Name, null))));
                   }
               });

            return sites;

        }

        public IEnumerable<IdNameModel> GetChatBotNames()
        {
            return dynamicRepository.Fetch<IdNameModel>(new GetChatbotNamesCriteria());
        }

        public ChatBotsConfigModel GetConfig(GetConfigViewModel paramModel)
        {
            var criteria = new GetConfigChatBotCriteria
            {
                CustID = paramModel.CusId,
                BotID = paramModel.BotId,
                SubAccID = paramModel.SubAccId
            };

            var (chatBotModel, functionConfigs) = dynamicRepository.FetchMultiple<ChatBotModel, ChatBotFunctionConfig>(criteria);

            return new ChatBotsConfigModel { ChatBotModel = chatBotModel.FirstOrDefault(), FunctionConfigs = functionConfigs };
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
                IsSpecialSite = HasSpecialSitePermission(customerId),
                BotIDs = paramModel.BotIDs
            };

            var rawData = dynamicRepository.Fetch<ChatBotModel>(criteria);

            return new ChatBotsMappingModel(criteria.TotalRows, rawData);
        }

        public Result UpdateFunctionsConfig(
            ChatBotFunctionConfig[] functionConfigs,
            int creatorId,
            string ip,
            int userId,
            int botId)
        {
            var criteria = new UpdFunctionsChatBotCriteria
            {
                Creator = creatorId,
                IP = ip,
                FunctionConfigs = functionConfigs,
                UserID = userId,
                BotID = botId
            };
            dynamicRepository.Execute(criteria);
            var result = new Result(criteria.ErrCode, criteria.ErrMsg);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Functions");
            }

            return result;
        }

        public Result UpdateLanguageConfig(int creatorId, string ip, int userId, int botId, string language)
        {
            var criteria = new UpdLanguageChatBotCriteria
            {
                Creator = creatorId,
                IP = ip,
                Language = language,
                UserID = userId,
                BotID = botId
            };
            dynamicRepository.Execute(criteria);
            var result = new Result(criteria.ErrCode, criteria.ErrMsg);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Language");
            }

            return result;
        }

        public Result UnlinkChatBot(int creatorId, string ip, int userId, int botId, long chatId, string language)
        {
            var criteria = new UnlinkChatBotCriteria
            {
                Creator = creatorId,
                IP = ip,
                Status = false,
                IsVerifyOTP = false,
                UserID = userId,
                BotID = botId,
            };
            dynamicRepository.Execute(criteria);
            var result = new Result(criteria.ErrCode, criteria.ErrMsg);

            if (result.IsSuccess)
            {
                result.Message = string.Format(Labels.UpdateSuccess, "Status");

                Task.Run(() => NotifyUnlink(userId, chatId, botId, language));
            }

            return result;
        }

        private static async Task NotifyUnlink(int userId, long chatId, int botId, string language)
        {
            try
            {
                await GetNotificationService().Unlink(userId, chatId, botId, language);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private static INotificationService GetNotificationService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var telegramClient = HttpClientFactory.Create();
            telegramClient.BaseAddress = AppSettings.TelegramBotClientUri;

            return new NotificationService(telegramClient);
        }

        public bool HasSpecialSitePermission(int customerId)
        {
            var accountingPolicy = dynamicRepository.Fetch<NAPAccPolicy>(new GetNapAccountingPolicy { CustID = customerId });
            var isHasAccessSpecialSites =
                accountingPolicy.Any(policy => policy.FuncName == CMSFunctionKeys.SpecialSites);

            return isHasAccessSpecialSites;
        }
    }
}