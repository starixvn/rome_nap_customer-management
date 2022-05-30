using Fanex.Common.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Resources;

namespace Fanex.Nap.CMS.ManageChatbots.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Controllers;
    using Services.ChatBotMapping;
    using ViewModels.ChatbotMapping;
    using System.Collections.Generic;
    using NAP.Core.Web.Attribute;
    using Infrastructure;
    using NAP.Core.Business;

    public class ChatBotMappingController : BaseController
    {
        private readonly IChatBotMappingService _service;

        public ChatBotMappingController() : this(null)
        {
        }

        public ChatBotMappingController(IChatBotMappingService service)
        {
            _service = service ?? new ChatBotMappingService();
        }

        // GET
        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotMapping, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var siteCurrencies = new ChatBotMappingViewModel(
                _service.GetCurrencies(),
                _service.GetSites()
            );

            return View("~/Areas/MangeChatbots/Views/ChatbotMapping/Index.cshtml", siteCurrencies);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotMapping, Policy = Constant.Policy.Read)]
        public JsonResult SearchChatBots(SearchChatBotsViewModel param)
        {
            var chatBots = _service.SearchChatBots(param, CurrentSession.UserId);

            return JsonData(chatBots);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotMapping, Policy = Constant.Policy.Read)]
        public ActionResult ChatBotConfig(GetConfigViewModel param)
        {
            var chatBotConfig = _service.GetConfig(param);

            chatBotConfig.OptionsLanguage = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "English", Value = "en-US" },
                new SelectListItem() { Text = "Chinese", Value = "zh-CN" },
                new SelectListItem() { Text = "Taiwan", Value = "zh-TW" },
                new SelectListItem() { Text = "Vietnamese", Value = "vi-VN" },
                new SelectListItem() { Text = "Thai", Value = "th-TH" },
                new SelectListItem() { Text = "Japanese", Value = "ja-JP" },
                new SelectListItem() { Text = "Korean", Value = "Korean" },
            };
            var optionLangue = chatBotConfig.OptionsLanguage.Find(c => c.Value == chatBotConfig.ChatBotModel.Language);
            if (optionLangue != null)
            {
                optionLangue.Selected = true;
            }

            return PartialView("~/Areas/MangeChatbots/Views/ChatbotMapping/_ConfigChatBot.cshtml", chatBotConfig);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotMapping, Policy = Constant.Policy.Read)]
        public JsonResult UpdateChatBotConfig(UpdateChatBotConfigViewModel viewModel)
        {
            if (!HasRight(CMSFunctionKeys.ChatbotMapping, Constant.Policy.Host))
            {
                return JsonData(new Result { Code = 500, Message = Labels.AccessDeniedMessage });
            }
            
            var updateLangResult = new Result();
            var updateFunctionResult = new Result();

            if (viewModel.ChatBotFunctionConfig != null)
            {
                updateFunctionResult = _service.UpdateFunctionsConfig(viewModel.ChatBotFunctionConfig,
                    CurrentSession.UserId,
                    CurrentContext.IP, viewModel.UserID);
            }

            if (!viewModel.Language.IsNullOrTrimEmpty())
            {
                updateLangResult = _service.UpdateLanguageConfig(CurrentSession.UserId,
                    CurrentContext.IP, viewModel.UserID, viewModel.BotId, viewModel.Language);
            }

            var result = new
            {
                updateFunctionResult,
                updateLangResult
            };

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ChatbotMapping, Policy = Constant.Policy.Read)]
        public JsonResult UnlinkBot(UnlinkBotViewModel viewModel)
        {
            if (!HasRight(CMSFunctionKeys.ChatbotMapping, Constant.Policy.Host))
            {
                return JsonData(new Result { Code = 500, Message = Labels.AccessDeniedMessage });
            }
            var updateLangResult = _service.UnlinkChatBot(CurrentSession.UserId,
                CurrentContext.IP, viewModel.UserID, viewModel.BotId, false, false);

            return JsonData(updateLangResult, JsonRequestBehavior.DenyGet);
        }
    }
}