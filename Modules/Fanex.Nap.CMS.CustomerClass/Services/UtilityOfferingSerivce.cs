namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using System.Collections.Generic;

    internal interface IUtilityOfferingSerivce
    {
        IEnumerable<UtilityOfferingModel> GetOfferings(SearchUtilityOfferingRequest request);

        AddOfferResponse AddOffer(AddEditUtilityOfferRequest request, int userId, string iP);

        Result UpdateOffer(AddEditUtilityOfferRequest request, int userId, string iP);

        Result DeleteOffer(int policyID, int userId, string iP);
    }
    internal class UtilityOfferingSerivce : ServiceBase, IUtilityOfferingSerivce
    {
        public UtilityOfferingSerivce() : base()
        {
        }

        public IEnumerable<UtilityOfferingModel> GetOfferings(SearchUtilityOfferingRequest request)
            => DynamicRepository.Fetch<UtilityOfferingModel>(new GetUtilityOfferCriteria(request));

        public AddOfferResponse AddOffer(AddEditUtilityOfferRequest request, int userId, string iP)
        {
            var criteria = new AddUtilityOfferCriteria(request, userId, iP);
            DynamicRepository.Execute(criteria);
            var message = criteria.ErrCode == 0
                ? Labels.AddUtilityOfferingSuccessfully ?? criteria.ErrMsg
                : criteria.ErrMsg;

            return new AddOfferResponse(criteria.PolicyID, criteria.ErrCode, message);
        }

        public Result UpdateOffer(AddEditUtilityOfferRequest request, int userId, string iP)
            => DoAction(new EditUtilityOfferCriteria(request, userId, iP), Labels.EditUtilityOfferSuccessfully);

        public Result DeleteOffer(int policyID, int userId, string iP)
            => DoAction(new DeleteUtilityOfferCriteria(policyID, userId, iP), Labels.DeleteUtilityOfferingSuccessfully);
    }
}
