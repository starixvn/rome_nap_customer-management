using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    public interface IVREngineService
    {
        VREngineMatrix GetVREngineMatrix();

        IEnumerable<IdNameModel> GetLeagues(string leagueGroupIds, string leagues);

        IEnumerable<VREngineRule> GetRules();

        Result AddNewRule(AddNewRuleViewModel viewModel);

        Result UpdateRule(int ruleId, AddNewRuleViewModel viewModel);

        Result DeleteRule(int ruleId, int userId, string ip);
    }
}
