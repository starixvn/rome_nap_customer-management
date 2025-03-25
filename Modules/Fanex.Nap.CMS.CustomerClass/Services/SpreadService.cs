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
    }

    internal class SpreadService : ServiceBase, ISpreadService
    {
        public SpreadService() : base()
        {
        }
        
        public AddSpreadRuleResponse AddRule(AddEditSpreadRuleRequest request, int userId, string iP)
        {
            var criteria = new AddSpreadRuleCriteria(request, userId, iP);
            DynamicRepository.Execute(criteria);
            var message = criteria.ErrCode == 0
                    ? Labels.AddSpreadSuccessfully ?? criteria.ErrMsg
                    : criteria.ErrMsg;

            return new AddSpreadRuleResponse(criteria.PolicyID, criteria.ErrCode, message);
        }

        public IEnumerable<SpreadRule> GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes)
            => DynamicRepository.Fetch<SpreadRule>(new GetSpreadRuleCriteria(request, string.Join(",", sportTypes.Select(sp => sp.Id))));

        public Result UpdateRule(AddEditSpreadRuleRequest request, int userId, string iP)
            => DoAction(new EditSpreadRuleCriteria(request, userId, iP), Labels.EditSpreadSuccessfully);

        public Result DeleteRule(int policyID, int userId, string iP)
            => DoAction(new DeleteSpreadCriteria(policyID, userId, iP), Labels.DeleteSpreadSuccessfully);

        public Result EnableRule(int policyID, bool enable, int userId, string iP)
            => DoAction(new EnableSpreadCriteria(policyID, enable, userId, iP), Labels.EditStatusSpreadSuccessfully);
    }
}
