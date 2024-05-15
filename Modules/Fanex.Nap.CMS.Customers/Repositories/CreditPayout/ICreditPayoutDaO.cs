namespace Fanex.Nap.CMS.Customers.Repositories.CreditPayout
{
    using Fanex.Nap.CMS.Customers.Criteria.CreditPayout;
    using Fanex.Nap.CMS.Customers.Models.CreditPayout;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    
    public interface ICreditPayoutDAO
    {
        IEnumerable<Match> GetMatches(GetMatchCiretia criteria);
        IEnumerable<PayoutPromotion> SearchPayout(SelPayoutPromotionCriteria criteria);
        PayoutPromotion ValidateVerifyCode(VerifySelPayoutPromotionCriteria criteria);
        Result UpdatePayoutPromotion(UpdPayoutPromotionCriteria criteria);
    }
}
