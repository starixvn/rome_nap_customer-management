using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.ManageChatbots.Constants;
using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotHistory
{
    public class ChatbotHistoryRepository : IChatbotHistoryRepository
    {
        public IEnumerable<ChatbotHistoryModel> GetChatbotHistories(ICriteria criteria)
        {
            var result = new[]
            {
                new ChatbotHistoryModel("Chatbot", "Yesterday Win Loss Turnover: 10,800 Gross Comm: 0 Agent Total: -4,690 Master WL: 0 Master Comm: 0 Master Total: 0 Company: 4,690", SendStatusConst.SendStatusList[0], new DateTime(2020,10,6)),
                new ChatbotHistoryModel("Chatbot", "Yesterday Win Loss Turnover: 10,800 Gross Comm: 0 Agent Total: -4,690 Master WL: 0 Master Comm: 0 Master Total: 0 Company: 4,690", SendStatusConst.SendStatusList[1],  new DateTime(2020,10,5)),
                new ChatbotHistoryModel("Customer", "/mwl", SendStatusConst.SendStatusList[1],  new DateTime(2020,10,4)),
                new ChatbotHistoryModel("Chatbot", "Yesterday Win Loss Turnover: 10,800 Gross Comm: 0 Agent Total: -4,690 Master WL: 0 Master Comm: 0 Master Total: 0 Company: 4,690", SendStatusConst.SendStatusList[1],  new DateTime(2020,10,3)),
                new ChatbotHistoryModel("Customer", "/mwl", SendStatusConst.SendStatusList[1],  new DateTime(2020,10,2)),
            };

            return result;
        }

        public IEnumerable<SelectListItem> GetAllFunctions(ICriteria criteria)
        {
            //using (var db = new ObjectDb(criteria.GetSettingKey()))
            //{
            //    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
            //    {
            //        while (reader.Read())
            //        {
            //             yield return new SelectListItem
            //            {
            //                Text = reader["FunctionName"].ToString(),
            //                Value = reader["FunctionId"].ToString()
            //            };
            //        }
            //    }
            //}

            return new[]
            {
                new SelectListItem{Text = "All", Value = "0"},
                new SelectListItem{Text = "Outstanding", Value = "1"},
                new SelectListItem{Text = "Today Win Loss", Value = "2"},
                new SelectListItem{Text = "Balance Info", Value = "3"},
                new SelectListItem{Text = "Yesterday Win Loss", Value = "4"},
                new SelectListItem{Text = "Personal Message", Value = "5"},
                new SelectListItem{Text = "Special Announcement", Value = "6"},
                new SelectListItem{Text = "Normal Announcement", Value = "7"},
                new SelectListItem{Text = "System Announcement", Value = "8"},
                new SelectListItem{Text = "Create new member from existing account", Value = "9"},
            };
        }
    }
}
