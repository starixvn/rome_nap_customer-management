namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;

    internal interface ISpreadService
    {
        IEnumerable<SpreadRule> GetSpreadRules(SearchSpreadRuleRequest request);

        AddSpreadRuleResponse AddRule(AddEditSpreadRuleRequest request, int userId, string iP);

        Result UpdateRule(AddEditSpreadRuleRequest request, int userId, string iP);

        Result DeleteRule(int policyID, int userId, string iP);

        Result EnableRule(int policyID, bool enable, int userId, string iP);
    }

    internal class SpreadService : ISpreadService
    {
        private readonly ISpreadDataAccess spreadDataAccess;
        private readonly IDynamicRepository dynamicRepository;

        public SpreadService() : this(new SpreadDataAccess(), new DynamicRepository())
        {
        }

        public SpreadService(ISpreadDataAccess spreadDataAccess, IDynamicRepository dynamicRepository)
        {
            this.spreadDataAccess = spreadDataAccess;
            this.dynamicRepository = dynamicRepository;
        }
        
        public AddSpreadRuleResponse AddRule(AddEditSpreadRuleRequest request, int userId, string iP)
            => spreadDataAccess.InsertRule(new AddSpreadRuleCriteria(request, userId, iP));

        public IEnumerable<SpreadRule> GetSpreadRules(SearchSpreadRuleRequest request)
        {
            return dynamicRepository.Fetch<SpreadRule>(new GetSpreadRuleCriteria(request));
        }

        public Result UpdateRule(AddEditSpreadRuleRequest request, int userId, string iP)
            => spreadDataAccess.DoAction(new EditSpreadRuleCriteria(request, userId, iP), Labels.EditSpreadSuccessfully);

        public Result DeleteRule(int policyID, int userId, string iP)
            => spreadDataAccess.DoAction(new DeleteSpreadCriteria(policyID, userId, iP), Labels.DeleteSpreadSuccessfully);

        public Result EnableRule(int policyID, bool enable, int userId, string iP)
            => spreadDataAccess.DoAction(new EnableSpreadCriteria(policyID, enable, userId, iP), Labels.EditStatusSpreadSuccessfully);
    }
}
