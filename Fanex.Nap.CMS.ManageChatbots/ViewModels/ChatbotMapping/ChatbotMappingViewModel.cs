namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Models;
    
    public class ChatBotMappingViewModel
    {
        public ChatBotMappingViewModel(IEnumerable<IdNameModel> currencies, IEnumerable<IdNameModel> sites, IEnumerable<IdNameModel> chatBotNames)
        {
            Currencies = currencies.ToList().ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            Sites = sites.ToList().ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            ChatBotNames = chatBotNames.ToList().ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
        }

        public IEnumerable<SelectListItem> Currencies { get; }

        public IEnumerable<SelectListItem> Sites { get; }
        
        public IEnumerable<SelectListItem> ChatBotNames { get; }
    }
}