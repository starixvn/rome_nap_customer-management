namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
    using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IVREngineService
    {
        VREngineMatrix GetVREngineMatrix();

        IEnumerable<IdNameModel> GetLeagues(
            string leagueGroupIds,
            string leagueGroupListNotIn,
            string leagues);

        VREngineModel GetRules();

        VREngineRule GetRule(int ruleId);

        Result AddNewRule(AddNewRuleViewModel viewModel);

        Result UpdateRule(int ruleId, AddNewRuleViewModel viewModel);

        Result DeleteRule(int ruleId, int userId, string ip);
    }
}
