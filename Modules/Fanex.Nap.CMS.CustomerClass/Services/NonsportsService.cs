namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Criteria.NonSports;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Requests.Nonsports;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using System.Collections.Generic;
    using System.Linq;

    internal interface INonsportsService
    {
        IEnumerable<NonsportModel> GetOfferings(SearchNonsportsRequest request, IEnumerable<IdNameModel> allproducts);

        AddOfferResponse AddOffer(AddEditNonsportsRequest request, int userId, string iP);

        Result UpdateOffer(AddEditNonsportsRequest request, IEnumerable<IdNameModel> allproducts, int userId, string iP);

        Result DeleteOffer(int policyID, IEnumerable<IdNameModel> allproducts, int userId, string iP);

        Result EnableOffer(EnableOfferingRequest request, int userId, string iP);

    }
    internal class NonsportsService : ServiceBase, INonsportsService
    {
        private const int AllProductId = 256;

        public NonsportsService() : base()
        {
        }

        public AddOfferResponse AddOffer(AddEditNonsportsRequest request, int userId, string iP)
        {
            var criteria = new AddNonSportsCriteria
            {
                CustomerClasses = request.CustomerClass,
                Sites = request.Sites,
                Products = request.Products,
                PolicyTitle = request.PolicyTitle,
                MaxBet = request.MaxBet,
                Enable = request.IsEnable,
                UserId = userId,
                IP = iP
            };
            DynamicRepository.Execute(criteria);
            var message = criteria.ErrCode == 0
                    ? Labels.AddNonSportsSuccessfully ?? criteria.ErrMsg
                    : criteria.ErrMsg;

            return new AddOfferResponse(criteria.PolicyID, criteria.ErrCode, message);
        }

        public Result DeleteOffer(int policyID, IEnumerable<IdNameModel> allproducts, int userId, string iP)
        {
            var criteria = new DeleteNonSportsCriteria
            {
                PolicyID = policyID,
                AllProducts = GetProductIds(allproducts),
                UserId = userId,
                IP = iP
            };

            return DoAction(criteria, Labels.DeleteNonSportSuccessfully);
        }

        public Result EnableOffer(EnableOfferingRequest request, int userId, string iP)
        {
            var criteria = new EnableNonSportsCriteria
            {
                PolicyID = request.PolicyId,
                Enable = request.Enable,
                UserId = userId,
                IP = iP
            };

            return DoAction(criteria, Labels.EditNonSportsSuccessfully);
        }

        public IEnumerable<NonsportModel> GetOfferings(SearchNonsportsRequest request, IEnumerable<IdNameModel> allproducts)
        {
            var criteria = new GetNonSportsCriteria
            {
                CustomerClasses = request.CustomerClasses,
                Products = request.Products,
                Sites = request.Sites,
                AllProducts = GetProductIds(allproducts)
            };

            return DynamicRepository.Fetch<NonsportModel>(criteria);
        }

        public Result UpdateOffer(AddEditNonsportsRequest request, IEnumerable<IdNameModel> allproducts, int userId, string iP)
        {
            var criteria = new EditNonSportsCriteria
            {
                CustomerClasses = request.CustomerClass,
                Sites = request.Sites,
                Products = request.Products,
                PolicyID = request.PolicyId,
                PolicyTitle = request.PolicyTitle,
                MaxBet = request.MaxBet,
                Enable = request.IsEnable,
                AllProducts = GetProductIds(allproducts),
                UserId = userId,
                IP = iP
            };

            return DoAction(criteria, Labels.EditNonSportsSuccessfully);
        }

        private string GetProductIds(IEnumerable<IdNameModel> allproducts)
        {
            var ids = allproducts.Select(pro => pro.Id).ToList();
            ids.Add(AllProductId);

            return string.Join(",", ids);
        }
    }
}
