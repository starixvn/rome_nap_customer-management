namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using Fanex.Data;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Common;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.CustomerClass.Resources;

    internal interface ISpreadDataAccess : IBaseRepository
    {
        AddSpreadRuleResponse InsertRule(AddSpreadRuleCriteria criteria);
    }

    internal class SpreadDataAccess : BaseRepository, ISpreadDataAccess
    {
        public AddSpreadRuleResponse InsertRule(AddSpreadRuleCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);

                var message = criteria.ErrCode == 0
                    ? Labels.AddSpreadSuccessfully ?? criteria.ErrMsg
                    : criteria.ErrMsg;

                return new AddSpreadRuleResponse(criteria.PolicyID, criteria.ErrCode, message);
            }
        }
    }
}
