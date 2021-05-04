using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Repositories.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    public class VREngineService : IVREngineService
    {
        private readonly IVREngineRepository repository;

        public VREngineService(IVREngineRepository repository = null)
        {
            this.repository = repository ?? new VREngineRepository();
        }

        public VREngineMatrix GetVREngineMatrix()
            => repository.GetVREngineMatrix(new GetVREngineMatrixCriteria());

        public IEnumerable<IdNameModel> GetLeagues(string leagueGroupIds, string leagues)
            => repository.GetLeagues(new GetVREngineLeagueCriteria(leagueGroupIds, leagues));

        public Result AddNewRule(AddNewRuleViewModel viewModel)
        {
            return new Result(0, "Add new rule successfully.");
        }
    }
}
