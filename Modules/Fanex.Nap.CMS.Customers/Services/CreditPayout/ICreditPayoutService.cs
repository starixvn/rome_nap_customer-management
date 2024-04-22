namespace Fanex.Nap.CMS.Customers.Services.CreditPayout
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Customers.Criteria.CreditPayout;
    using Fanex.Nap.CMS.Customers.Models.CreditPayout;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface ICreditPayoutService
    {
        IEnumerable<SelectListItem> GetMatches(int typeId);
        SearchPayoutResponse SearchPayout(SelPayoutPromotionCriteria request);
        PayoutPromotion ValidateVerifyCode(VerifySelPayoutPromotionCriteria request);
        IEnumerable<Result> UpdPayoutPromotion(UpdPayoutPromotionCriteria request);
        Result VerifyPayoutPromotion(UpdPayoutPromotionCriteria request);
    }
}
