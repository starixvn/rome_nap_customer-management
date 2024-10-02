namespace Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour
{
    internal class DeletePeakHourCriteria : CustomerClassCommandBaseCriteria
    {
        public DeletePeakHourCriteria(int rangeID, int userId, string iP)
        {
            RangeID = rangeID;
            base.UserId = userId;
            base.IP = iP;
        }

        public int RangeID { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CCPeakHourRangerDel";
    }
}
