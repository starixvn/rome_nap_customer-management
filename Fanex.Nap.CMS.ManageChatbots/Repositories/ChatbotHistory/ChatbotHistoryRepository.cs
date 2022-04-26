using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.ManageChatbots.Constants;
using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
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
        public IEnumerable<ChatbotHistoryItem> GetChatbotHistories(ChatbotHistoryCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = db.Query<ChatbotHistoryItem>(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                criteria.TotalRows = Convert.ToInt32(outputParams["totalrows"]);

                return result;
            }
        }

        public IEnumerable<SelectListItem> GetAllFunctions(ICriteria criteria)
        {
            var functions = new List<SelectListItem>();
            functions.Add(new SelectListItem { Text = "All", Value = "0" });

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {           
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        functions.Add(new SelectListItem
                        {
                            Text = reader["FunctionName"].ToString(),
                            Value = reader["FunctionID"].ToString()
                        });
                    }
                }
                return functions;
            }
        }
    }
}
