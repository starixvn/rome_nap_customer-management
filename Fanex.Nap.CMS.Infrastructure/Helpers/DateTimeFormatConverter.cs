namespace Fanex.Nap.CMS.Infrastructure.Helpers
{
    using Newtonsoft.Json.Converters;

    public class DateTimeFormatConverter : IsoDateTimeConverter
    {
        public DateTimeFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
