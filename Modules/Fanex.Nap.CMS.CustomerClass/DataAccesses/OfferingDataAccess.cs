namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Repositories;

    internal interface IOfferingDataAccess : IBaseRepository
    {
        AddOfferResponse AddOffer(AddUtilityOfferCriteria criteria);
    }
    internal class OfferingDataAccess : BaseRepository, IOfferingDataAccess
    {
        public AddOfferResponse AddOffer(AddUtilityOfferCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();
                var outputParams = db.GetOuputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var policyID = Converter.ToInt(outputParams["policyid"]);
                var message = code == 0
                    ? Labels.AddUtilityOfferingSuccessfully ?? Converter.ToString2(outputParams["errmsg"])
                    : Converter.ToString2(outputParams["errmsg"]);

                return new AddOfferResponse(policyID, code, message);
            }
        }
    }
}
