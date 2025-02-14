namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotHistory
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotHistory;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotHistory;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class ChatbotHistoryRepository : IChatbotHistoryRepository
    {
        public IEnumerable<ChatbotHistoryItem> GetChatbotHistories(ChatbotHistoryCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var result = db.Query<ChatbotHistoryItem>(criteria);
                var outputParams = db.GetOutputParams();
                criteria.TotalRows = Convert.ToInt32(outputParams["totalrows"]);

                return result;
            }
        }

        public IEnumerable<FunctionModel> GetAllFunctions(ICriteria criteria)
        {
            var functions = new List<FunctionModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {           
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        functions.Add(new FunctionModel
                        {
                            FunctionId = Convert.ToInt32(reader["FunctionID"]),
                            FunctionType = Convert.ToInt32(reader["Type"]),
                            FunctionName = reader["FunctionName"].ToString()
                        });      
                    }
                }
                return functions;
            }
        }
    }
}
