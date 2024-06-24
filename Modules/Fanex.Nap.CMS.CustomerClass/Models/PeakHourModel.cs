namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System;

    internal class PeakHourModel
    {
        public PeakHourModel(int rangeID, byte weekDay, TimeSpan startRange, TimeSpan endRange, byte isTest, int lastModifiedUser, DateTime lastModifiedTime)
        {
            RangeID = rangeID;
            WeekDay = weekDay;
            StartRange = startRange;
            EndRange = endRange;
            IsTest = isTest;
            LastModifiedUser = lastModifiedUser;
            LastModifiedTime = lastModifiedTime;
        }

        public int RangeID { get; set; }
        public byte WeekDay { get; set; }
        public TimeSpan StartRange { get; set; }
        public TimeSpan EndRange { get; set; }
        public byte IsTest { get; set; }
        public int LastModifiedUser { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
