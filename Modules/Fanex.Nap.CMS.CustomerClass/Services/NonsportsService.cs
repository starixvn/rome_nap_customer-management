namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Criteria.NonSports;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Requests.Nonsports;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;
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
    internal class NonsportsService : INonsportsService
    {
        private readonly INonSportsDataAccess nonSportsDataAccess;
        private readonly IDynamicRepository dynamicRepository;
        private const int AllProductId = 256;

        public NonsportsService() : this(new DynamicRepository(), new NonSportsDataAccess())
        {
        }

        public NonsportsService(IDynamicRepository dynamicRepository, INonSportsDataAccess nonSportsDataAccess)
        {
            this.dynamicRepository = dynamicRepository;
            this.nonSportsDataAccess = nonSportsDataAccess ?? new NonSportsDataAccess();
        }

        public AddOfferResponse AddOffer(AddEditNonsportsRequest request, int userId, string iP)
        {
            var criteria = new AddNonSportsCriteria(userId, iP)
            {
                CustomerClasses = request.CustomerClass,
                Sites = request.Sites,
                Products = request.Products,
                PolicyTitle = request.PolicyTitle,
                MaxBet = request.MaxBet,
                Enable = request.IsEnable
            };

            return nonSportsDataAccess.AddOffer(criteria);
        }

        public Result DeleteOffer(int policyID, IEnumerable<IdNameModel> allproducts, int userId, string iP)
        {
            var criteria = new DeleteNonSportsCriteria(userId, iP)
            {
                PolicyID = policyID,
                AllProducts = GetProductIds(allproducts)
            };

            return nonSportsDataAccess.DoAction(criteria, Labels.DeleteNonSportSuccessfully);
        }

        public Result EnableOffer(EnableOfferingRequest request, int userId, string iP)
        {
            var criteria = new EnableNonSportsCriteria(userId, iP)
            {
                PolicyID = request.PolicyId,
                Enable = request.Enable
            };

            return nonSportsDataAccess.DoAction(criteria, Labels.EditNonSportsSuccessfully);
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

            return dynamicRepository.Fetch<NonsportModel>(criteria);
        }

        public Result UpdateOffer(AddEditNonsportsRequest request, IEnumerable<IdNameModel> allproducts, int userId, string iP)
        {
            var criteria = new EditNonSportsCriteria(userId, iP)
            {
                CustomerClasses = request.CustomerClass,
                Sites = request.Sites,
                Products = request.Products,
                PolicyID = request.PolicyId,
                PolicyTitle = request.PolicyTitle,
                MaxBet = request.MaxBet,
                Enable = request.IsEnable,
                AllProducts = GetProductIds(allproducts)
            };

            return nonSportsDataAccess.DoAction(criteria, Labels.EditNonSportsSuccessfully);
        }

        private string GetProductIds(IEnumerable<IdNameModel> allproducts)
        {
            var ids = allproducts.Select(pro => pro.Id).ToList();
            ids.Add(AllProductId);

            return string.Join(",", ids);
        }
    }
}
