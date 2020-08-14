namespace Fanex.Nap.CMS.Infrastructure.Helpers
{
    using Newtonsoft.Json;
    using System;

    public class TimeSpanFormatConverter : JsonConverter
    {
        private readonly string format;

        public TimeSpanFormatConverter(string format)
        {
            this.format = format;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            TimeSpan parsedTimeSpan;
            TimeSpan.TryParseExact((string)reader.Value, format, null, out parsedTimeSpan);
            return parsedTimeSpan;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var timeSpan = (TimeSpan)value;
            var timespanFormatted = $"{timeSpan.ToString(format)}";
            writer.WriteValue(timespanFormatted);
        }
    }
}
