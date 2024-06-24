namespace Fanex.Nap.CMS.CustomerClass.Requests.PeakHour
{
    using System;
    
    public class AddPeakHourRequest
    {
        public byte WeekDay { get; set; }
        public string StartRange { get; set; }
        public string EndRange { get; set; }
    }
}
