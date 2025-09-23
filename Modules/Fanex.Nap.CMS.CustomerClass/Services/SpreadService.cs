namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;

    internal interface ISpreadService
    {
        IEnumerable<SpreadRule> GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes);

        AddSpreadRuleResponse AddRule(AddEditSpreadRuleRequest request, int userId, string iP);

        Result UpdateRule(AddEditSpreadRuleRequest request, int userId, string iP);

        Result DeleteRule(int policyID, int userId, string iP);

        Result EnableRule(int policyID, bool enable, int userId, string iP);

        Result Upload(IEnumerable<SpreadSettingExcelValidModel> spreadSettingExcels, int userId, string ip);
    }

    internal class SpreadService : ServiceBase, ISpreadService
    {
        private byte SpreadType { get; }

        public SpreadService(byte spreadType) : base()
        {
            SpreadType = spreadType;
        }

        public AddSpreadRuleResponse AddRule(AddEditSpreadRuleRequest request, int userId, string iP)
        {
            var criteria = new AddSpreadRuleCriteria
            {
                PolicyID = request.PolicyID,
                PolicyTitle = request.PolicyTitle,
                SportType = request.SportType,
                Spread = request.Spread,
                CustomerClasses = request.CustomerClass,
                Currencies = request.Currencies,
                Sites = request.Sites,
                Platforms = request.Platforms,
                Enable = request.Enable,
                LeagueGroups = request.LeagueGroups,
                IsLive = request.IsLive,
                Type = SpreadType,
                UserId = userId,
                IP = iP
            };
            DynamicRepository.Execute(criteria);
            var message = criteria.ErrCode == 0
                    ? Labels.AddSpreadSuccessfully ?? criteria.ErrMsg
                    : criteria.ErrMsg;

            return new AddSpreadRuleResponse(criteria.PolicyID, criteria.ErrCode, message);
        }

        public IEnumerable<SpreadRule> GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes)
        {
            var hasSpread = float.TryParse(request.Spread, out var spread);
            var criteria = new GetSpreadRuleCriteria
            {
                CustomerClasses = request.CustomerClass,
                Sites = request.Site,
                Currencies = request.Currency,
                LeagueCheck = request.LeagueGroups,
                LeagueGroups = string.Empty,
                Spread = hasSpread ? spread : (float?)null,
                SportTypes = string.Join(",", sportTypes.Select(sp => sp.Id)),
                Type = SpreadType
            };
            return DynamicRepository.Fetch<SpreadRule>(criteria);
        }

        public Result UpdateRule(AddEditSpreadRuleRequest request, int userId, string iP)
        {
            var criteria = new EditSpreadRuleCriteria
            {
                PolicyID = request.PolicyID,
                PolicyTitle = request.PolicyTitle,
                SportType = request.SportType,
                Spread = request.Spread,
                CustomerClasses = request.CustomerClass,
                Currencies = request.Currencies,
                Sites = request.Sites,
                Platforms = request.Platforms,
                Enable = request.Enable,
                LeagueGroups = request.LeagueGroups,
                IsLive = request.IsLive,
                Type = SpreadType,
                UserId = userId,
                IP = iP
            };

            return DoAction(criteria, Labels.EditSpreadSuccessfully);
        }

        public Result DeleteRule(int policyID, int userId, string iP)
        {
            var criteria = new DeleteSpreadCriteria
            {
                PolicyID = policyID,
                Type = SpreadType,
                UserId = userId,
                IP = iP
            };

            return DoAction(criteria, Labels.DeleteSpreadSuccessfully);
        }

        public Result EnableRule(int policyID, bool enable, int userId, string iP)
        {
            var criteria = new EnableSpreadCriteria
            {
                PolicyID = policyID,
                Enable = enable,
                Type = SpreadType,
                UserId = userId,
                IP = iP
            };

            return DoAction(criteria, Labels.EditStatusSpreadSuccessfully);
        }

        public Result Upload(IEnumerable<SpreadSettingExcelValidModel> spreadSettingExcels, int userId, string ip)
        {
            return new Result { Message = "Successfully" };
        }
    }
}
