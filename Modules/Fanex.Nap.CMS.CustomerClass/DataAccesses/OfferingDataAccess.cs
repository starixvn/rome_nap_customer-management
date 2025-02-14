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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);
                var message = criteria.ErrCode == 0
                    ? Labels.AddUtilityOfferingSuccessfully ?? criteria.ErrMsg
                    : criteria.ErrMsg;

                return new AddOfferResponse(criteria.PolicyID, criteria.ErrCode, message);
            }
        }
    }
}
