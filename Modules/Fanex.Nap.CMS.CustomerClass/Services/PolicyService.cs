namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal interface IPolicyService
    {
        IEnumerable<Policy> GetPolicies(IEnumerable<IdNameModel> sportTypes, string policyTitle = null);

        PolicyItem GetPolicy(IEnumerable<IdNameModel> sportTypes, int policyId);

        IEnumerable<IdNameModel> GetPolicyTitles(string sportTypes);

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

        public IEnumerable<Policy> GetPolicies(IEnumerable<IdNameModel> sportTypes, string policyTitle = null)
            => GetPolicyList(sportTypes, policyTitle: policyTitle)
            .GroupBy(p => new { p.PolicyId, p.PolicyTitle, p.SportType })
            .Select(grp => new Policy(
                grp.Key.PolicyId,
                grp.Key.PolicyTitle,
                grp.Key.SportType,
                grp.FirstOrDefault()?.Rule == null ? Enumerable.Empty<Rule>() : grp.Where(p => p.Rule?.Enable == true).Select(p => p.Rule)))
            .OrderBy(plc => plc.PolicyTitle);

        public PolicyItem GetPolicy(IEnumerable<IdNameModel> sportTypes, int policyId)
            => GetPolicyList(sportTypes, policyId)
            .GroupBy(p => new { p.PolicyId, p.PolicyTitle, p.SportType })
            .Select(grp => new PolicyItem(
                grp.Key.PolicyId,
                grp.Key.PolicyTitle,
                grp.Key.SportType,
                grp.FirstOrDefault()?.Rule == null ? Enumerable.Empty<int>() : grp.Select(p => p.Rule.RuleId)))
            .FirstOrDefault();

        private IEnumerable<Policy> GetPolicyList(IEnumerable<IdNameModel> sportTypes, int policyId = 0, string policyTitle = null)
        {
            var leagues = filterService.GetLeagueGroups();
            var leagueGroupDict = ToLeagueGroupDictionary(leagues);
            var sportTypeIds = string.Join(",", sportTypes.Select(sp => sp.Id));
            var policies = policyDataAccess.GetPolicies(new GetPolicyCriteria(policyId, policyTitle, BuildLeageGroupXML(leagues), sportTypeIds));

            foreach (var item in policies)
            {
                if (item.Rule != null)
                {
                    if (!leagueGroupDict.TryGetValue(item.Rule.SportType, out var leagueCount))
                    {
                        leagueCount = leagueGroupDict[0];
                    }

                    item.Rule.IsAllLeagueGroups = leagueCount == NumberOfItemsSeparatedByComma(item.Rule.LeagueGroups);
                    item.Rule.SportTypeName = sportTypes.FirstOrDefault(sp => sp.Id == item.Rule.SportType)?.Name;
                }

                yield return item;
            }
        }

        private IDictionary<short, int> ToLeagueGroupDictionary(IEnumerable<LeagueGroup> leagues)
        {
            var leagueGroupDict = new Dictionary<short, int>();

            var leagueGroups = leagues
                .GroupBy(l => l.SportType)
                .ToDictionary(l => l.Key, l => l.Count());
            foreach (var kvp in leagueGroups)
            {
                leagueGroupDict[kvp.Key] = kvp.Value + leagueGroups[0];
            }

            return leagueGroupDict;
        }
        private string BuildLeageGroupXML(IEnumerable<LeagueGroup> leagues)
        {
            var xmlBuilder = new StringBuilder();

            foreach (var lg in leagues)
            {
                xmlBuilder
                .Append("<r GroupID=\"").Append(lg.Id).Append("\"")
                .Append(" GroupName=\"").Append(WebUtility.HtmlEncode(lg.Name)).Append("\"")
                .Append(" SportType=\"").Append(lg.SportType).Append("\"/>");
            }

            return $"<ROOT>{xmlBuilder.ToString()}</ROOT>";
        }

        private static int NumberOfItemsSeparatedByComma(string value)
            => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

        public IEnumerable<IdNameModel> GetPolicyTitles(string sportTypes)
            => policyDataAccess.GetPolicyTitles(new GetPolicyTitleCriteria(sportTypes));

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
