namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Fanex.Data.Repository;

    public class GetGroupDownlinesCriteria : CriteriaBase
    {
        public GetGroupDownlinesCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_MonitorList";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
