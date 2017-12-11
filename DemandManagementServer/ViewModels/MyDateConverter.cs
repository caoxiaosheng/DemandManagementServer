using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DemandManagementServer.ViewModels
{
    public class MyDateConverter : DateTimeConverterBase
    {
        private static readonly IsoDateTimeConverter DtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DtConverter.WriteJson(writer, value, serializer);
        }
    }
}
