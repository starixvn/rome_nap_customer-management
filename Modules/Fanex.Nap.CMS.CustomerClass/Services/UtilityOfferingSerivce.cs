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
        {
            var criteria = new GetUtilityOfferCriteria(request)
            {
                CustomerClasses = request.CustomerClass,
                Sites = request.Site,
                Currencies = request.Currency
            };

            return DynamicRepository.Fetch<UtilityOfferingModel>(criteria);
        }

        public AddOfferResponse AddOffer(AddEditUtilityOfferRequest request, int userId, string iP)
        {
            var criteria = new AddUtilityOfferCriteria
            {
                PolicyID = request.PolicyId,
                PolicyTitle = request.PolicyTitle,
                CustomerClasses = request.CustomerClass,
                Currencies = request.Currencies,
                Sites = request.Sites,
                UtilityOffering = request.UtilityOffering,
                UserId = userId,
                IP = iP
            };
            DynamicRepository.Execute(criteria);
            var message = criteria.ErrCode == 0
                ? Labels.AddUtilityOfferingSuccessfully ?? criteria.ErrMsg
                : criteria.ErrMsg;

            return new AddOfferResponse(criteria.PolicyID, criteria.ErrCode, message);
        }

        public Result UpdateOffer(AddEditUtilityOfferRequest request, int userId, string iP)
        {
            var criteria = new EditUtilityOfferCriteria
            {
                PolicyID = request.PolicyId,
                PolicyTitle = request.PolicyTitle,
                CustomerClasses = request.CustomerClass,
                Currencies = request.Currencies,
                Sites = request.Sites,
                UtilityOffering = request.UtilityOffering,
                UserId = userId,
                IP = iP
            };

            return DoAction(criteria, Labels.EditUtilityOfferSuccessfully);
        }

        public Result DeleteOffer(int policyID, int userId, string iP)
        {
            var criteria = new DeleteUtilityOfferCriteria
            {
                PolicyID = policyID,
                UserId = userId,
                IP = iP
            };

            return DoAction(criteria, Labels.DeleteUtilityOfferingSuccessfully);
        }
    }
}
