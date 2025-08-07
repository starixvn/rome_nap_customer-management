namespace Fanex.Nap.CMS.DefaultMemberBetLimit
{
    using Fanex.Nap.CMS.DefaultMemberBetLimit.Criteria;
    using Fanex.Nap.CMS.DefaultMemberBetLimit.Models;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.CMS.DefaultMemberBetLimit.Criteria;
    using Fanex.NAP.CMS.DefaultMemberBetLimit.Models;
    using Fanex.NAP.CMS.DefaultMemberBetLimit.Requests;
    using System.Collections.Generic;

    public interface IService
    {
        IndexViewModel GetIndexViewModel();

        IEnumerable<SearchModel> Search(SearchRequest request);

        Result Insert(InsertRequest request);

        Result Update(UpdateRequest request);

        Result Delete(DeleteRequest request);
    }

    public class Service : ServiceBase, IService
    {
        private readonly INapSession currentSession;
        private readonly ICurrentContext currentContext;

        public Service(INapSession currentSession, ICurrentContext currentContext)
        {
            this.currentSession = currentSession;
            this.currentContext = currentContext;
        }

        public IndexViewModel GetIndexViewModel()
        {
            var betLimitTypes = GetRepositoryData<BetLimitTypeModel>(new GetAllBetLimitTypeCriteria());
            return new IndexViewModel
            {
                BetLimitTypes = betLimitTypes
            };
        }

        public IEnumerable<SearchModel> Search(SearchRequest request)
        {
            var criteria = new SearchCriteria
            {
                UserId = currentSession.UserId,
                BetLimitTypeId = request.BetLimitTypeId,
                Username = request.Username
            };
            return GetRepositoryData<SearchModel>(criteria);
        }

        public Result Insert(InsertRequest request)
        {
            var criteria = new InsertCriteria
            {
                BetLimitTypeId = request.BetLimitTypeId,
                Username = request.Username.Trim(),
                MaxBet = request.MaxBet,
                MaxPerMatch = request.MaxPerMatch,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };
            Repository.Execute(criteria);
            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }

        public Result Update(UpdateRequest request)
        {
            var criteria = new UpdateCriteria
            {
                BetLimitTypeId = request.BetLimitTypeId,
                CustId = request.CustId,
                MaxBet = request.MaxBet,
                MaxPerMatch = request.MaxPerMatch,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };
            Repository.Execute(criteria);
            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }

        public Result Delete(DeleteRequest request)
        {
            var criteria = new DeleteCriteria
            {
                BetLimitTypeId = request.BetLimitTypeId,
                CustId = request.CustId,
                IsDownlineAffected = request.IsDownlineAffected,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };
            Repository.Execute(criteria);
            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }
    }
}
