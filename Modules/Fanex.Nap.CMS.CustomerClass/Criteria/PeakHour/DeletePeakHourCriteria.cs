namespace Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour
{
    internal class DeletePeakHourCriteria : CommandCriteriaBase
    {
        public DeletePeakHourCriteria(int rangeID, int userId, string iP) : base(userId, iP)
        {
            RangeID = rangeID;
        }

        public int RangeID { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CCPeakHourRangerDel";
    }
}
