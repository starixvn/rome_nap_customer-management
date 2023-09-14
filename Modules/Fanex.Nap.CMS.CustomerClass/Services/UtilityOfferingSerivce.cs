namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using System;
    using System.Collections.Generic;

    internal interface IUtilityOfferingSerivce
    {
        IEnumerable<UtilityOfferingModel> GetOfferings(SearchUtilityOfferingRequest request);
    }
    public class UtilityOfferingSerivce : IUtilityOfferingSerivce
    {
        private readonly IDynamicRepository dynamicRepository;

        public UtilityOfferingSerivce() : this(new DynamicRepository())
        {
        }

        public UtilityOfferingSerivce(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<UtilityOfferingModel> GetOfferings(SearchUtilityOfferingRequest request)
        {
            return new List<UtilityOfferingModel>() {
                new UtilityOfferingModel(1, "Heavy", "200, 300, 301", "All", "AllCredit", "Parlay Boost", 2, DateTime.Today),
                new UtilityOfferingModel(2, "Bad Performance", "2000", "56", "All", "Parlay Boost", 2, DateTime.Today)
            };
        }
    }
}
