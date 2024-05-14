namespace Fanex.Nap.CMS.Customers.Repositories.CreditPayout
{
    using Fanex.Nap.CMS.Customers.Criteria.CreditPayout;
    using Fanex.Nap.CMS.Customers.Models.CreditPayout;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;
    
    public interface ICreditPayoutDAO
    {
        IEnumerable<SelectListItem> GetMatches(GetMatchCiretia criteria);
        IEnumerable<PayoutPromotion> SearchPayout(SelPayoutPromotionCriteria criteria);
        PayoutPromotion ValidateVerifyCode(VerifySelPayoutPromotionCriteria criteria);
        Result UpdatePayoutPromotion(UpdPayoutPromotionCriteria criteria);
    }
}
