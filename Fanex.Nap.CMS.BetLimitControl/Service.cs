namespace Fanex.NAP.CMS.BetLimitControl
{
    using System.Linq;
    using Fanex.Nap.CMS.BetLimitControl.Criteria;
    using Fanex.Nap.CMS.BetLimitControl.Models;
    using Fanex.Nap.CMS.BetLimitControl.Request;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.NAP.CMS.BetLimitControl.Criteria;
    using Fanex.NAP.CMS.BetLimitControl.Models;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    public interface IService
    {
        IndexViewModel GetIndexViewModel();

        SearchModel Search(SearchRequest request);
    }

    public class Service : IService
    {
        private readonly IServiceBase serviceBase;
        private readonly ICurrentSession currentSession;
        private readonly ICurrentContext currentContext;

        public Service(ICurrentSession currentSession, ICurrentContext currentContext)
            : this(currentSession, currentContext, null)
        {
        }

        public Service(ICurrentSession currentSession, ICurrentContext currentContext, IServiceBase serviceBase)
        {
            this.currentSession = currentSession;
            this.currentContext = currentContext;
            this.serviceBase = serviceBase ?? new ServiceBase();
        }

        public IndexViewModel GetIndexViewModel()
        {
            var currencies = serviceBase.GetRepositoryData<CurrencyModel>(new GetAllCurrencyCriteria());
            var sites = serviceBase.GetRepositoryData<SiteModel>(new GetSiteByPermissionCriteria(currentSession.UserId));
            var betLimitTypes = serviceBase.GetRepositoryData<BetLimitTypeModel>(new GetAllBetLimitTypeCriteria());
            return new IndexViewModel
            {
                Currencies = currencies,
                Sites = sites,
                BetLimitTypes = betLimitTypes
            };
        }

        public SearchModel Search(SearchRequest request)
        {
            var criteria = new SearchCriteria
            {
                UserId = currentSession.UserId,
                LevelID = request.LevelId,
                Currency = request.Currency,
                SiteID = request.SiteId,
                BetLimitTypeID = request.BetLimitTypeID,
                Username = request.Username,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
            };
            var customerBetLimitControls = serviceBase.GetRepositoryData<CustomerBetLimitModel>(criteria);

            var result = new SearchModel
            {
                TotalItems = criteria.TotalItems,
                CustomerBetLimitControls = customerBetLimitControls.GroupBy(control => control.Username)
            };

            return result;
        }
    }
}