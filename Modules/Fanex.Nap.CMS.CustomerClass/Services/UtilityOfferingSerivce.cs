namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;

    internal interface IUtilityOfferingSerivce
    {
        IEnumerable<UtilityOfferingModel> GetOfferings(SearchUtilityOfferingRequest request);

        AddOfferResponse AddOffer(AddEditUtilityOfferRequest request, int userId, string iP);

        Result UpdateOffer(AddEditUtilityOfferRequest request, int userId, string iP);

        Result DeleteOffer(int policyID, int userId, string iP);
    }
    internal class UtilityOfferingSerivce : IUtilityOfferingSerivce
    {
        private readonly IOfferingDataAccess offeringDataAccess;
        public UtilityOfferingSerivce() : this(new OfferingDataAccess())
        {
        }

        public UtilityOfferingSerivce(IOfferingDataAccess offeringDataAccess)
        {
            this.offeringDataAccess = offeringDataAccess ?? new OfferingDataAccess();
        }

        public IEnumerable<UtilityOfferingModel> GetOfferings(SearchUtilityOfferingRequest request)
            => offeringDataAccess.ExecuteReader<UtilityOfferingModel>(new GetUtilityOfferCriteria(request));

        public AddOfferResponse AddOffer(AddEditUtilityOfferRequest request, int userId, string iP)
            => offeringDataAccess.AddOffer(new AddUtilityOfferCriteria(request, userId, iP));

        public Result UpdateOffer(AddEditUtilityOfferRequest request, int userId, string iP)
            => offeringDataAccess.DoAction(new EditUtilityOfferCriteria(request, userId, iP), Labels.EditUtilityOfferSuccessfully);

        public Result DeleteOffer(int policyID, int userId, string iP)
            => offeringDataAccess.DoAction(new DeleteUtilityOfferCriteria(policyID, userId, iP), Labels.DeleteUtilityOfferingSuccessfully);
    }
}
