namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotMapping
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;
    using System;
    using System.Data;
    using Fanex.Data;
    public class ChatbotMappingRepository : IChatbotMappingRepository
    {
        public IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetCurrency(reader);
                    }
                }
            }
        }
        private static IdNameModel GetCurrency(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["exchangeid"]),
                Name = reader["currency"].ToString()
            };
    }
}