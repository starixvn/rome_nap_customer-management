namespace Fanex.NAP.CMS.BetLimitControl
{
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.NAP.CMS.BetLimitControl.Criteria;
    using Fanex.NAP.CMS.BetLimitControl.Models;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    public interface IService
    {
        IndexViewModel GetIndexViewModel();
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
            var sites = serviceBase.GetRepositoryData<SiteModel>(new GetAllSiteCriteria());
            var betLimitTypes = serviceBase.GetRepositoryData<BetLimitTypeModel>(new GetAllBetLimitTypeCriteria());
            return new IndexViewModel
            {
                Currencies = currencies,
                Sites = sites,
                BetLimitTypes = betLimitTypes
            };
        }
    }
}
