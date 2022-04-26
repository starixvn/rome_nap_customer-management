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
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = db.Query<ChatbotHistoryItem>(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                criteria.TotalRows = Convert.ToInt32(outputParams["totalrows"]);

                return result;
            }
        }

        public IEnumerable<IdNameModel> GetAllFunctions(ICriteria criteria)
        {
            var functions = new List<IdNameModel>();
            functions.Add(new IdNameModel { Name = "All", Id = 0 });

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {           
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        functions.Add(new IdNameModel
                        {
                            Name = reader["FunctionName"].ToString(),
                            Id = Convert.ToInt32(reader["FunctionID"])
                        });
                    }
                }
                return functions;
            }
        }
    }
}
