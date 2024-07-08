namespace Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour
{
    internal class EditPeakHourCriteria : AddPeakHourCriteria
    {
        public EditPeakHourCriteria(int rangeId, byte weekDay, string startRange, string endRange, int userId, string ip) : base(weekDay, startRange, endRange, userId, ip)
        {
            RangeID = rangeId;
        }

        public override string GetSettingKey()
            => "NAP_LM_CCPeakHourRangerUpd";
    }
}
