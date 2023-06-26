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
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();
                var outputParams = db.GetOuputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var message = code == 0
                    ? Labels.AddSpreadSuccessfully ?? Converter.ToString2(outputParams["errmsg"])
                    : Converter.ToString2(outputParams["errmsg"]);

                return new AddSpreadRuleResponse(criteria.PolicyID, code, message);
            }
        }
    }
}
