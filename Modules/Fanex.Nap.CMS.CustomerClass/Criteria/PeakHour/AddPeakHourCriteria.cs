namespace Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour
{
    using System;

    internal class AddPeakHourCriteria : CommandCriteriaBase
    {
        public AddPeakHourCriteria(byte weekDay, string startRange, string endRange, int userId, string ip) : base(userId, ip)
        {
            WeekDay = weekDay;
            StartRange = startRange;
            EndRange = endRange;
        }

        public byte WeekDay { get; set; }
        public string StartRange { get; set; }
        public string EndRange { get; set; }
        public int RangeID { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CCPeakHourRangerIns";
    }
}
