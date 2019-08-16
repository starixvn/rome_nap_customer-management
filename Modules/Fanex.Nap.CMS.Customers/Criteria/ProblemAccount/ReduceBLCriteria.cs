namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    using Data.Repository;

    public class ReduceBLCriteria : NonQueryCriteriaBase
    {
        public ReduceBLCriteria(
            string usernames,
            byte roleId,
            int maxPerEvent,
            int maxPerBet,
            int userId,
            string ip) : base(usernames, roleId, userId, ip)
        {
            MaxPerEvent = maxPerEvent;
            MaxPerBet = maxPerBet;
        }

        public int MaxPerEvent { get; }

        [SpParam(SqlParamName = "maxbet")]
        public int MaxPerBet { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_ReduceBetLimit";

        public override bool IsValid()
            => base.IsValid() && MaxPerEvent >= 0 && MaxPerBet >= 0;
    }
}
