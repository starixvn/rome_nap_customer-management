namespace Fanex.Nap.CMS.Customers.Repositories.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using System.Collections.Generic;

    public class LimitDailyPTUpdateRepository : ILimitDailyPTUpdateRepository
    {
        public IEnumerable<string> GetSites()
        {
            //TODO: Add query to db
            return new[] { "123", "456", "789", "1111111", "333333", "555555" };
        }

        public IEnumerable<string> GetProducts()
        {
            //TODO: Add query to db
            return new[] { "Happy 5", "Number Game", "Happy 5 Linked Game" };
        }

        public IEnumerable<string> GetCurrencies()
        {
            //TODO: Add query to db
            return new[] { "USD", "VND", "BTC", "BTAAAAAA", "AAACCCCCBBB" };
        }

        public IEnumerable<LimitDailyPTUpdateItem> GetLimitDailyPTUpdateList(GetLimitDailyPTUpdateListCriteria criteria)
        {
            //TODO: Add query to db
            var limitDailyPTUpdateList = new List<LimitDailyPTUpdateItem>
            {
                new LimitDailyPTUpdateItem {Username = "Test", Site = "733333", Currency = "UUS", Product = "Macau Games", Type = "SMA Line"},
                new LimitDailyPTUpdateItem {Username = "Test1", Site = "733333", Currency = "", Product = "Macau Games", Type = "Site"},
                new LimitDailyPTUpdateItem {Username = "Test2", Site = "", Currency = "HKK", Product = "Macau Games", Type = "Currency"},
            };
            return limitDailyPTUpdateList;
        }
       
    }
}
