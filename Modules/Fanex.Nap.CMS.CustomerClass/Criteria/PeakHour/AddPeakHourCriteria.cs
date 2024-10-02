namespace Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour
{
    internal class AddPeakHourCriteria : CustomerClassCommandBaseCriteria
    {
        public AddPeakHourCriteria(byte weekDay, string startRange, string endRange, int userId, string ip)
        {
            WeekDay = weekDay;
            StartRange = startRange;
            EndRange = endRange;
            base.UserId = userId;
            base.IP = ip;
        }

        public AddPeakHourCriteria() { }

        public byte WeekDay { get; set; }
        public string StartRange { get; set; }
        public string EndRange { get; set; }
        public int RangeID { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CCPeakHourRangerIns";
    }
}
