namespace Fanex.Nap.CMS.Customers.Criteria.BetList
{
    using System;
    using Data.Repository;

    public class GetAbnormalBetTicketCriteria : CriteriaBase
    {
        public GetAbnormalBetTicketCriteria(
            int custId,
            DateTime from,
            DateTime to,
            int pageIndex,
            int pageSize)
        {
            CustId = custId;
            FromDate = from;
            ToDate = to;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_VoidedTransactionList";

        public override bool IsValid()
            => true;
    }
}
