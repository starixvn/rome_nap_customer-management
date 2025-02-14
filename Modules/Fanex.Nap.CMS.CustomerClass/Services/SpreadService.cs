namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using System.Linq;

    internal interface ISpreadService
    {
        IEnumerable<SpreadRule> GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes);

        AddSpreadRuleResponse AddRule(AddEditSpreadRuleRequest request, int userId, string iP);

        Result UpdateRule(AddEditSpreadRuleRequest request, int userId, string iP);

        Result DeleteRule(int policyID, int userId, string iP);

        Result EnableRule(int policyID, bool enable, int userId, string iP);
    }

    internal class SpreadService : ISpreadService
    {
        private readonly ISpreadDataAccess spreadDataAccess;

        public SpreadService() : this(new SpreadDataAccess())
        {
        }

        public SpreadService(ISpreadDataAccess spreadDataAccess)
        {
            this.spreadDataAccess = spreadDataAccess;
        }
        
        public AddSpreadRuleResponse AddRule(AddEditSpreadRuleRequest request, int userId, string iP)
            => spreadDataAccess.InsertRule(new AddSpreadRuleCriteria(request, userId, iP));

        public IEnumerable<SpreadRule> GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes)
        {
            return spreadDataAccess.ExecuteReader<SpreadRule>(new GetSpreadRuleCriteria(request, string.Join(",", sportTypes.Select(sp => sp.Id))));
        }

        public Result UpdateRule(AddEditSpreadRuleRequest request, int userId, string iP)
            => spreadDataAccess.DoAction(new EditSpreadRuleCriteria(request, userId, iP), Labels.EditSpreadSuccessfully);

        public Result DeleteRule(int policyID, int userId, string iP)
            => spreadDataAccess.DoAction(new DeleteSpreadCriteria(policyID, userId, iP), Labels.DeleteSpreadSuccessfully);

        public Result EnableRule(int policyID, bool enable, int userId, string iP)
            => spreadDataAccess.DoAction(new EnableSpreadCriteria(policyID, enable, userId, iP), Labels.EditStatusSpreadSuccessfully);
    }
}
