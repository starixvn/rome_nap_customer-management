
namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatBotMapping
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Web.Mvc;


    public class ChatBotMappingViewModel
    {
        public ChatBotMappingViewModel(IEnumerable<SelectListItem> currencies,IEnumerable<SelectListItem> sites) {
          
            Currencies = currencies;
            Sites = sites;
        }
        public IEnumerable<SelectListItem> Currencies { get; }
        
        public IEnumerable<SelectListItem> Sites { get; }
    }
}