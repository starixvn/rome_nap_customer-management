namespace Fanex.Nap.CMS.CustomerClass.Requests.PeakHour
{

    public class AddEditPeakHourRequest
    {
        public int RangeId { get; set; }
        public byte WeekDay { get; set; }
        public string StartRange { get; set; }
        public string EndRange { get; set; }
    }
}
