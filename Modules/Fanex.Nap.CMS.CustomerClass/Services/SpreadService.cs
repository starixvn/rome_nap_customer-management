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
        private readonly IDynamicRepository dynamicRepository;

        private byte SpreadType { get; }

        public SpreadService(byte spreadType) : this(new SpreadDataAccess(), new DynamicRepository())
        {
            SpreadType = spreadType;
        }

        public SpreadService(ISpreadDataAccess spreadDataAccess, IDynamicRepository dynamicRepository)
        {
            this.spreadDataAccess = spreadDataAccess;
            this.dynamicRepository = dynamicRepository;
        }
        
        public AddSpreadRuleResponse AddRule(AddEditSpreadRuleRequest request, int userId, string iP)
            => spreadDataAccess.InsertRule(new AddSpreadRuleCriteria(request, userId, iP, SpreadType));

        public IEnumerable<SpreadRule> GetSpreadRules(SearchSpreadRuleRequest request, IEnumerable<IdNameModel> sportTypes)
        {
            return dynamicRepository.Fetch<SpreadRule>(new GetSpreadRuleCriteria(request, string.Join(",", sportTypes.Select(sp => sp.Id)), SpreadType));
        }

        public Result UpdateRule(AddEditSpreadRuleRequest request, int userId, string iP)
            => spreadDataAccess.DoAction(new EditSpreadRuleCriteria(request, userId, iP, SpreadType), Labels.EditSpreadSuccessfully);

        public Result DeleteRule(int policyID, int userId, string iP)
            => spreadDataAccess.DoAction(new DeleteSpreadCriteria(policyID, userId, iP, SpreadType), Labels.DeleteSpreadSuccessfully);

        public Result EnableRule(int policyID, bool enable, int userId, string iP)
            => spreadDataAccess.DoAction(new EnableSpreadCriteria(policyID, enable, userId, iP, SpreadType), Labels.EditStatusSpreadSuccessfully);
    }
}
