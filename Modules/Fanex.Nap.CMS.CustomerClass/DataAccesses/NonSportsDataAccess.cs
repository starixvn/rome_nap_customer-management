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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);
                var message = criteria.ErrCode == 0
                    ? Labels.AddNonSportsSuccessfully ?? criteria.ErrMsg
                    : criteria.ErrMsg;

                return new AddOfferResponse(criteria.PolicyID, criteria.ErrCode, message);
            }
        }
    }
}
