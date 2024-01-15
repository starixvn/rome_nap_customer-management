namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.CustomerClass.Criteria.NonSports;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Repositories;

    internal interface INonSportsDataAccess : IBaseRepository
    {
        AddOfferResponse AddOffer(AddNonSportsCriteria criteria);

    }
    internal class NonSportsDataAccess : BaseRepository, INonSportsDataAccess
    {
        public AddOfferResponse AddOffer(AddNonSportsCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();
                var outputParams = db.GetOuputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var policyID = Converter.ToInt(outputParams["policyid"]);
                var message = code == 0
                    ? Labels.AddNonSportsSuccessfully ?? Converter.ToString2(outputParams["errmsg"])
                    : Converter.ToString2(outputParams["errmsg"]);

                return new AddOfferResponse(policyID, code, message);
            }
        }
    }
}
