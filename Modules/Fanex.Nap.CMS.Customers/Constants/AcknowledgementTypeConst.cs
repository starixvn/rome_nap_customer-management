namespace Fanex.Nap.CMS.Customers.Constants
{
    using System.Collections.Generic;
    using Infrastructure.Models;
    using Resources;

    public static class AcknowledgementTypeConst
    {
        public const int NotAcknowledged = 1;
        public const int Acknowledged = 2;

        public static IEnumerable<IdNameModel> AcknowledgementTypeList()
        {
            yield return new IdNameModel { Id = 0, Name = ProblemAccountResource.All };
            yield return new IdNameModel { Id = NotAcknowledged, Name = ProblemAccountCreditDepositResource.NotAcknowledged };
            yield return new IdNameModel { Id = Acknowledged, Name = nameof(Acknowledged) };
        }
    }

}
