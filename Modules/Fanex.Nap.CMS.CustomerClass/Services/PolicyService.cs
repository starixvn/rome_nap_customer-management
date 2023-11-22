namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal interface IPolicyService
    {
        IEnumerable<Policy> GetPolicies();

        PolicyItem GetPolicy(int policyId);

        IEnumerable<IdNameModel> GetPolicyTitles(short sportType = 0);

        Result AddPolicy(AddPolicyRequest request);

        Result EditPolicy(int policyId, AddPolicyRequest request);

        CheckPolicyResponse CheckPolicies(DeletePolicyRequest request);

        Result DeletePolicies(DeletePolicyRequest request);
    }

    internal class PolicyService : IPolicyService
    {
        private readonly IFilterService filterService;
        private readonly IPolicyDataAccess policyDataAccess;

        public PolicyService(IFilterService filterService = null, IPolicyDataAccess policyDataAccess = null)
        {
            this.filterService = filterService ?? new FilterService();
            this.policyDataAccess = policyDataAccess ?? new PolicyDataAccess();
        }

        public IEnumerable<Policy> GetPolicies()
            => GetPolicyList()
            .GroupBy(p => new { p.PolicyId, p.PolicyTitle, p.SportType })
            .Select(grp => new Policy(
                grp.Key.PolicyId,
                grp.Key.PolicyTitle,
                grp.Key.SportType,
                grp.FirstOrDefault()?.Rule == null ? Enumerable.Empty<Rule>() : grp.Where(p => p.Rule.Enable == true).Select(p => p.Rule)))
            .OrderBy(plc => plc.PolicyTitle);

        public PolicyItem GetPolicy(int policyId)
            => GetPolicyList(policyId)
            .GroupBy(p => new { p.PolicyId, p.PolicyTitle, p.SportType })
            .Select(grp => new PolicyItem(
                grp.Key.PolicyId,
                grp.Key.PolicyTitle,
                grp.Key.SportType,
                grp.FirstOrDefault()?.Rule == null ? Enumerable.Empty<int>() : grp.Select(p => p.Rule.RuleId)))
            .FirstOrDefault();

        private IEnumerable<Policy> GetPolicyList(int policyId = 0)
        {
            var sportTypes = SportTypeConst.SportTypes;
            var leagueGroupDict = ToLeagueGroupDictionary();
            var policies = policyDataAccess.GetPolicies(new GetPolicyCriteria(policyId));

            foreach (var item in policies)
            {
                if (item.Rule != null)
                {
                    if (!leagueGroupDict.TryGetValue(item.Rule.SportType, out var leagueCount))
                    {
                        leagueCount = leagueGroupDict[0];
                    }

                    item.Rule.IsAllLeagueGroups = leagueCount == NumberOfItemsSeparatedByComma(item.Rule.LeagueGroups);
                    item.Rule.SportTypeName = sportTypes[item.Rule.SportType];
                }

                yield return item;
            }
        }

        private IDictionary<short, int> ToLeagueGroupDictionary()
        {
            var leagueGroupDict = new Dictionary<short, int>();

            var leagueGroups = filterService.GetLeagueGroups()
                .GroupBy(l => l.SportType)
                .ToDictionary(l => l.Key, l => l.Count());
            foreach (var kvp in leagueGroups)
            {
                leagueGroupDict[kvp.Key] = kvp.Value + leagueGroups[0];
            }

            return leagueGroupDict;
        }

        private static int NumberOfItemsSeparatedByComma(string value)
            => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

        public IEnumerable<IdNameModel> GetPolicyTitles(short sportType = 0)
            => policyDataAccess.GetPolicyTitles(new GetPolicyTitleCriteria(sportType));

        public Result AddPolicy(AddPolicyRequest request)
            => policyDataAccess.DoAction(new AddPolicyCriteria(request), Labels.AddPolicySuccessfully);

        public Result EditPolicy(int policyId, AddPolicyRequest request)
            => policyDataAccess.DoAction(new EditPolicyCriteria(policyId, request), Labels.EditPolicySuccessfully);


        public CheckPolicyResponse CheckPolicies(DeletePolicyRequest request)
            => policyDataAccess.CheckPolicy(new CheckPolicyDelCriteria(request));

        public Result DeletePolicies(DeletePolicyRequest request)
            => policyDataAccess.DoAction(new DeletePolicyCriteria(request), Labels.DeletePolicySuccessfully);
    }
}
