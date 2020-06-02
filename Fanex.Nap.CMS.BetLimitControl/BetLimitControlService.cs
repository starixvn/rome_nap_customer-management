using System.Linq;
using Fanex.Nap.CMS.BetLimitControl.Criteria;
using Fanex.Nap.CMS.BetLimitControl.Models;
using Fanex.Nap.CMS.BetLimitControl.Request;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.NAP.Core.Web.Context;
using Fanex.NAP.Core.Web.Session;

namespace Fanex.Nap.CMS.BetLimitControl
{
    public interface IBetLimitControlService
    {
        SearchModel Search(SearchRequest request);
    }

    public class BetLimitControlService : IBetLimitControlService
    {
        private readonly ICurrentSession currentSession;
        private readonly ICurrentContext currentContext;
        private readonly ServiceBase serviceBase;

        public BetLimitControlService(ICurrentSession session, ICurrentContext context)
        {
            currentSession = session;
            currentContext = context;
            serviceBase = serviceBase ?? new ServiceBase();
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